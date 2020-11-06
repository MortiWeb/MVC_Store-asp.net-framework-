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
                return RedirectToAction("Main");
            }
            PageVM page;
            using (Db db = new Db())
            {
                page = new PageVM(db.Pages.Where(p => p.Slug == slug).FirstOrDefault());
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
                sidebar = new SidebarVM(db.Sidebars.First());
            }
            return PartialView("_SideBarPartial", sidebar);
        }
    }
}