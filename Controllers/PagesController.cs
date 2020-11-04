using MVC_Store.Models.Data;
using MVC_Store.Models.ViewModels.Pages;
using MVC_Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Controllers
{
    public class PagesController : Controller
    {
        // GET: AllProducts(main page)
        public ActionResult Main()
        {
            List<ProductVM> products;
            using(Db db = new Db())
            {
                products = db.Products.Where(p => true).ToArray().Select(p => new ProductVM(p)).ToList();
            }
            return View(products);
        }
        // GET: 
        public ActionResult ServicePage(string slug)
        {
            if(string.IsNullOrEmpty(slug))
            {
                return RedirectToAction("Main");
            }
            PageVM page;
            using (Db db = new Db())
            {
                page = db.Pages.Where(p => p.Slug == slug).Select(p => new PageVM(p)).FirstOrDefault();
                if (page == null)
                {
                    return RedirectToAction("Main");
                }
                if(page.HasSidebar == true)
                {
                    ViewBag.SideBar = "OK";
                }
                else
                {
                    ViewBag.SideBar = "NONE";
                }
            }
            return View(page);
        }
    }
}