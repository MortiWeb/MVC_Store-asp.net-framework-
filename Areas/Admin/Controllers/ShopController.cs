using MVC_Store.Models.Data;
using MVC_Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace MVC_Store.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            List<CategoryVM> categories;
            using (Db db = new Db())
            {
                categories = db.Categories.ToArray().OrderBy(c => c.Sorting).Select(c => new CategoryVM(c)).ToList();
            }
            return View(categories);
        }

        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            string id;
            using (Db db = new Db())
            {
                if (db.Categories.Any(c => c.Name == catName))
                    return "titletaken";

                CategoryDTO dto = new CategoryDTO();
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;
                db.Categories.Add(dto);
                db.SaveChanges();
                id = dto.Id.ToString();
            }
            return id;
        }

        // POST: Admin/Shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, string id)
        {
            int _id;
            bool result = Int32.TryParse(id, out _id);
            if (!result) return "Changing the category name failed. Cannot parse the ID.";
            using (Db db = new Db())
            {
                if (db.Categories.Any(c => c.Name == newCatName))
                    return "titletaken";

                CategoryDTO dto = db.Categories.Find(_id);
                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();
                db.SaveChanges();
            }
            return "";
        }

        // POST: Save JS sorting(custom pages sorting)
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                CategoryDTO dto;

                foreach (var catId in id)
                {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }

        // GET: Admin/Shop/DeleteCategory
        [HttpGet]
        public ActionResult DeleteCategory(int? id)
        {
            if (id == null) return RedirectToAction("Categories");
            using (Db db = new Db())
            {
                CategoryDTO dto = db.Categories.Find(id);
                if (dto == null)
                {
                    TempData["EM"] = "This page dose not exist!";
                    return RedirectToAction("Categories");
                }
                db.Categories.Remove(dto);
                db.SaveChanges();
            }
            TempData["SM"] = "You have deleted the page";
            return RedirectToAction("Categories");
        }

        // GET: Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            ProductVM model = new ProductVM();
            GetSomeDataForValidate(model);
            return View(model);
        }

        // POST: Admin/Shop/AddProduct
        [HttpPost]
        public ActionResult AddProduct(ProductVM model, IEnumerable<HttpPostedFileBase> file)
        {
            int id;
            string sId;
            var checkFile = file.First();
            string styleNumber = model.StyleNumber;
            using (Db db = new Db())
            {
                #region Validation
                bool checkUnique = db.Products.Any(p => p.StyleNumber == styleNumber);

                if (!ModelState.IsValid || checkUnique)
                {
                    GetSomeDataForValidate(model);

                    if (checkUnique)
                    {
                        ModelState.AddModelError("", "That Style Number is taken, enter unique number");
                    }
                    return View(model);
                }

                if (checkFile != null)
                {
                    bool checkContType = file.All(f => (f.ContentType.ToLower() == "image/jpeg") || (f.ContentType.ToLower() == "image/jpg") || (f.ContentType.ToLower() == "image/png"));
                    if (!checkContType)
                    {
                        GetSomeDataForValidate(model);
                        ModelState.AddModelError("", "The images were not uploaded, image extension must be jpeg, jpg or png");
                        return View(model);
                    }
                }
                #endregion

                string slug = model.Name.Replace(" ", "-") + "-" + styleNumber;
                ProductDTO dto = new ProductDTO();
                dto.Name = model.Name;
                dto.Slug = slug.ToLower();
                dto.Description = model.Description;
                dto.ManufacturerId = model.ManufacturerId;
                dto.StyleNumber = styleNumber.ToLower();
                dto.Price = model.Price;
                if (checkFile != null)
                {
                    dto.ImageName = file.First().FileName;
                }
                dto.Categories = db.Categories.Where(ca => model.SelectedCategoryIds.Contains(ca.Id)).ToList();

                db.Products.Add(dto);
                db.SaveChanges();

                id = dto.Id;
                sId = dto.Id.ToString();
            }
            //TempData["SM"] = "You have added a new product";

            #region Upload image

            string originalDir = $"{Server.MapPath(@"\")}Images\\Uploads\\Products\\{sId}\\Gallery";
            string thumbsDir = $"{Server.MapPath(@"\")}Images\\Uploads\\Products\\{sId}\\Gallery\\Thumbs";
            if (!Directory.Exists(thumbsDir))
            {
                Directory.CreateDirectory(thumbsDir);

                if (checkFile != null)
                {
                    foreach (var f in file)
                    {
                        try
                        {
                            f.SaveAs($"{originalDir}\\{f.FileName}");
                        }
                        catch
                        {
                            TempData["SM"] = "You have added a new product";
                            TempData["EM"] = "But the images failed to save";
                            return RedirectToAction("EditProduct", id);
                        }
                    }
                    WebImage img;
                    foreach (var f in file)
                    {
                        img = new WebImage(f.InputStream);
                        img.Resize(321, 321).Crop(1, 1);
                        try
                        {
                            img.Save($"{thumbsDir}\\{f.FileName}");
                        }
                        catch
                        {
                            TempData["SM"] = "You have added a new product";
                            TempData["EM"] = "But the images failed to save";
                            return RedirectToAction("EditProduct", id);
                        }
                    }
                }
            }
            /*var originalDir = new DirectoryInfo($"{Server.MapPath(@"\")}Images\\Uploads");
            string[] pathArr =
            {
                Path.Combine(originalDir.ToString(), "Products"),
                Path.Combine(originalDir.ToString(), $"Products\\{sId}"),
                Path.Combine(originalDir.ToString(), $"Products\\{sId}\\Thumbs"),
                Path.Combine(originalDir.ToString(), $"Products\\{sId}\\Gallery"),
                Path.Combine(originalDir.ToString(), $"Products\\{sId}\\Gallery\\Thumbs")
            };
            foreach (var p in pathArr)
            {
                if (!Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }
            }

            string imageName = file.FileName;
            var path = string.Format($"{pathArr[1]}\\{imageName}");
            var path2 = string.Format($"{pathArr[2]}\\{imageName}"); */
            

            /*file.SaveAs(path);

            WebImage image = new WebImage(file.InputStream);
            image.Resize(320, 320);
            image.Save(path2);*/

            #endregion

            TempData["SM"] = "You have added a new product";
            return RedirectToAction("Products");
        }

        // GET: Admin/Shop/Products
        [HttpGet]
        public ActionResult Products(int? page, int? catId)
        {
            List<ProductVM> listOfProductVM;
            var pageNum = page ?? 1;

            using (Db db = new Db())
            {
                listOfProductVM = db.Products.Where(p => catId == null || catId == 0 ||
                p.Categories.Any(c => c.Id == catId)).ToArray().Select(p => new ProductVM(p)).ToList();
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                ViewBag.SelectedCat = catId.ToString();
            }

            ViewBag.OnePageOfProducts = listOfProductVM.ToPagedList(pageNum, 3);

            return View(listOfProductVM);
        }

        // GET: Admin/Shop/EditProduct
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ProductVM model;
            using (Db db = new Db())
            {
                var p = db.Products.Find(id);
                if (p == null)
                {
                    TempData["EM"] = "This product does not exist";
                    return RedirectToAction("Products");
                }
                model = new ProductVM(p);
                model.SelectedCategoryIds = p.Categories.Where(c => true).Select(c => c.Id).ToList();
            }
            GetSomeDataForValidate(model, true);
            return View(model);
        }

        // POST: Admin/Shop/EditProduct
        [HttpPost]
        public ActionResult EditProduct(ProductVM model, IEnumerable<HttpPostedFileBase> file)
        {
            int id = model.Id;
            var checkFile = file.First();
            string styleNumber = model.StyleNumber.ToLower();

            using (Db db = new Db())
            {
                #region Validation
                bool checkUnique = db.Products.Where(p => p.Id != id).Any(p => p.StyleNumber == styleNumber);

                if (!ModelState.IsValid || checkUnique)
                {
                    GetSomeDataForValidate(model, true);

                    if (checkUnique)
                    {
                        ModelState.AddModelError("", "That Style Number is taken, enter unique number");
                    }
                    return View(model);
                }

                if (checkFile != null)
                {
                    bool checkContType = file.All(f => (f.ContentType.ToLower() == "image/jpeg") || (f.ContentType.ToLower() == "image/jpg") || (f.ContentType.ToLower() == "image/png"));
                    if (!checkContType)
                    {
                        GetSomeDataForValidate(model, true);
                        ModelState.AddModelError("", "The images were not uploaded, image extension must be jpeg, jpg or png");
                        return View(model);
                    }
                }
                #endregion

                string slug = model.Name.Replace(" ", "-") + "-" + styleNumber;
                ProductDTO dto = db.Products.Find(id);
                dto.Name = model.Name;
                dto.Slug = slug.ToLower();
                dto.Description = model.Description;
                dto.ManufacturerId = model.ManufacturerId;
                dto.StyleNumber = styleNumber;
                dto.Price = model.Price;
                if (checkFile != null)
                {
                    dto.ImageName = file.First().FileName;
                }
                dto.Categories = db.Categories.Where(ca => model.SelectedCategoryIds.Contains(ca.Id)).ToList();
                db.SaveChanges();
            }

            #region Upload images

            if (checkFile != null)
            {
                var originalDir = Path.Combine($"{Server.MapPath(@"\")}Images\\Uploads\\Products", id.ToString(), "Gallery");
                if (Directory.Exists(originalDir))
                {
                    var originalImages = Directory.EnumerateFiles(originalDir);
                    if (originalImages.Count() > 0)
                    {
                        foreach (var path in originalImages)
                        {
                            try
                            {
                                System.IO.File.Delete(path);
                            }
                            catch
                            {
                                TempData["SM"] = "You have edited the product";
                                TempData["EM"] = "But the images failed to save";
                                return RedirectToAction("EditProduct", id);
                            }
                        }
                    }
                    foreach (var f in file)
                    {
                        try
                        {
                            f.SaveAs(string.Format($"{originalDir}\\{f.FileName}"));
                        }
                        catch
                        {
                            TempData["SM"] = "You have edited the product";
                            TempData["EM"] = "But the images failed to save";
                            return RedirectToAction("EditProduct", id);
                        }
                    }
                }
                var thumbsDir = Path.Combine($"{Server.MapPath(@"\")}Images\\Uploads\\Products", id.ToString(), "Gallery\\Thumbs");
                if (Directory.Exists(thumbsDir))
                {
                    var thumbsImages = Directory.EnumerateFiles(thumbsDir);
                    if (thumbsImages.Count() > 0)
                    {
                        foreach (var path in thumbsImages)
                        {
                            try
                            {
                                System.IO.File.Delete(path);
                            }
                            catch
                            {
                                TempData["SM"] = "You have edited the product";
                                TempData["EM"] = "But the images failed to save";
                                return RedirectToAction("EditProduct", id);
                            }
                        }
                    }
                    WebImage img;
                    foreach (var f in file)
                    {
                        img = new WebImage(f.InputStream);
                        img.Resize(321, 321).Crop(1, 1);
                        try
                        {
                            img.Save(string.Format($"{thumbsDir}\\{f.FileName}"));
                        }
                        catch
                        {
                            TempData["SM"] = "You have edited the product";
                            TempData["EM"] = "But the images failed to save";
                            return RedirectToAction("EditProduct", id);
                        }
                    }
                }

            }

            #endregion
            TempData["SM"] = "You have edited the product";
            return RedirectToAction("Products");
        }

        // GET: Admin/Shop/DeleteProduct
        public ActionResult DeleteProduct(int id)
        {
            using (Db db = new Db())
            {
                ProductDTO dto = db.Products.Find(id);
                if (dto == null)
                {
                    TempData["EM"] = "This product dose not exist!";
                    RedirectToAction("Products");
                }

                var originalDir = Path.Combine($"{Server.MapPath(@"\")}Images\\Uploads\\Products", id.ToString());
                if (Directory.Exists(originalDir))
                {
                    try
                    {
                        Directory.Delete(originalDir, true);
                    }
                    catch
                    {
                        TempData["EM"] = "The product failed to delete";
                        return RedirectToAction("Products");
                    }
                }
                db.Products.Remove(dto);
                db.SaveChanges();
            }
            TempData["SM"] = "You have deleted the product";
            return RedirectToAction("Products");
        }


        void GetSomeDataForValidate(ProductVM model, bool isEdit = false)
        {
            using (Db db = new Db())
            {
                model.DDListCategories = new MultiSelectList(db.Categories.ToList(), "Id", "Name", model.SelectedCategoryIds);
                model.DDListCountries = new SelectList(db.Countries.ToList(), "Id", "Name");

                if (isEdit == true)
                {
                    var puthImg = Path.Combine($"{Server.MapPath(@"\")}Images\\Uploads\\Products", model.Id.ToString(), "Gallery\\Thumbs");
                    if (Directory.Exists(puthImg))
                    {
                        model.GalleryImages = Directory.EnumerateFiles(puthImg).Select(fn => Path.GetFileName(fn)).ToList();
                        if (model.GalleryImages.Count() == 0)
                            model.GalleryImages = null;
                    }
                }
            }
        }
    }
}