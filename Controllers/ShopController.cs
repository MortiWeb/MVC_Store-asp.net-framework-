using MVC_Store.Models.Data;
using MVC_Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Controllers
{
    public class ShopController : Controller
    {
        // GET: AllProducts(main page)
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

        // GET: shop/
        public ActionResult ProductCats()
        {

            return View();
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

    }
}