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
        // GET: Pages/ServicePage/{slug}
        public ActionResult ServicePage(string slug)
        {
            if(string.IsNullOrEmpty(slug))
            {
                return RedirectToAction("Main", "Shop");
            }
            PageVM page;
            using (Db db = new Db())
            {
                var prd = db.Pages.Where(p => p.Slug == slug).FirstOrDefault();
                if (prd == null) return new HttpNotFoundResult();

                page = new PageVM(prd);
                
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
        public ActionResult ServiceMenuPartial()
        {
            List<PageVM> listPagesForMenu;
            using (Db db = new Db())
            {
                listPagesForMenu = db.Pages.Where(p => true).ToArray().OrderBy(p => p.Sorting).Select(p => new PageVM(p)).ToList();
            }
            return PartialView("_ServiceMenuPartial", listPagesForMenu);
        }
        public ActionResult SideBarPartial()
        {
            SidebarVM sidebar; 
            using (Db db = new Db())
            {
                var sb = db.Sidebars.First();
                if (sb == null) return new HttpNotFoundResult();

                sidebar = new SidebarVM(sb);
            }
            return PartialView("_SideBarPartial", sidebar);
        }
    }
}