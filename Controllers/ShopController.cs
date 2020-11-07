using MVC_Store.Models.Data;
using MVC_Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop/Main/{slug} (main page)
        public ActionResult Main(string slug)
        {
            List<ProductVM> products;
            using (Db db = new Db())
            {
                var category = db.Categories.Where(c => c.Slug == slug).FirstOrDefault();
                if(category == null)
                {
                    ViewBag.Title = "Main store page";
                    products = db.Products.ToArray().Select(p => new ProductVM(p)).ToList();
                    return View(products);
                }
                ViewBag.Title = category.Name + " Category";
                products = db.Products.Where(p => p.Categories.Any(c => c.Id == category.Id)).ToArray().Select(p => new ProductVM(p)).ToList();
            }
            return View(products);
        }

        public ActionResult CategoryPartial(string slug)
        {
            List<CategoryVM> cat;
            using (Db db = new Db())
            {
                cat = db.Categories.ToArray().OrderBy(c => c.Sorting).Select(c => new CategoryVM(c)).ToList();
            }
            return PartialView("_CategoryPartial", cat);
        }

        // GET: Shop/DetailsProduct/{id}
        public ActionResult DetailsProduct(string slug)
        {
            ProductVM product;
            using (Db db = new Db())
            {
                var prd = db.Products.Where(p => p.Slug == slug).FirstOrDefault();
                if(prd == null) 
                    return new HttpNotFoundResult();
                
                product = new ProductVM(prd);

                var puthImg = Path.Combine($"{Server.MapPath(@"\")}Images\\Uploads\\Products", product.Id.ToString(), "Gallery\\Thumbs");
                if (Directory.Exists(puthImg))
                {
                    product.GalleryImages = Directory.EnumerateFiles(puthImg).Select(fn => Path.GetFileName(fn)).ToList();
                    if (product.GalleryImages.Count() == 0)
                        product.GalleryImages = null;
                }
            }
            return View(product);
        }
    }
}