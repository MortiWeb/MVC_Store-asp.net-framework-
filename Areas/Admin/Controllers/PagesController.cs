using MVC_Store.Models.Data;
using MVC_Store.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageVM> PageList;
            using (Db db = new Db())
            {
                PageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            return View(PageList);
        }
        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                string slug;
                PagesDTO dto = new PagesDTO();

                if (db.Pages.Any(x => x.Article == model.Article.ToLower()))
                {
                    ModelState.AddModelError("", "That URL already exist");
                    return View(model);
                }
                else
                {
                    dto.Article = model.Article.Replace(" ", "-").ToLower();
                }

                dto.Title = model.Title.ToUpper();
                slug = model.Title.Replace(" ", "-").ToLower();
                slug += "-" + dto.Article;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;
                db.Pages.Add(dto);
                db.SaveChanges();
            }
            TempData["SM"] = "You have added a new page.";
            return RedirectToAction("Index");
        }

        // GET: Admin/Pages/EditPage
        [HttpGet]
        public ActionResult EditPage(int? id)
        {
            if (id == null) return RedirectToAction ("Index");
            PageVM model;
            using (Db db = new Db())
            {
                PagesDTO dto = db.Pages.Find(id);
                if (dto == null)
                {
                    TempData["EM"] = "This page dose not exist!";
                    return RedirectToAction("Index");
                }
                model = new PageVM(dto);
            }
            return View(model);
        }

        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                string slug;
                PagesDTO dto = db.Pages.Find(model.Id);

                if (db.Pages.Where(x => x.Id != model.Id).Any(x => x.Article == model.Article.ToLower()))
                {
                    ModelState.AddModelError("", "That URL already exist");
                    return View(model);
                }
                else
                {
                    dto.Article = model.Article.Replace(" ", "-").ToLower();
                }

                dto.Title = model.Title.ToUpper();
                slug = model.Title.Replace(" ", "-").ToLower();
                slug += "-" + dto.Article;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                db.SaveChanges();
            }
            TempData["SM"] = "You have edited the page";
            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/PageDetails
        public ActionResult PageDetails(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            PageVM model;
            using (Db db = new Db())
            {
                PagesDTO dto = db.Pages.Find(id);
                if (dto == null)
                {
                    TempData["EM"] = "This page dose not exist!";
                    return RedirectToAction("Index");
                }
                model = new PageVM(dto);
            }
            return View(model);
        }

        // GET: Admin/Pages/Index
        public ActionResult DeletePage(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            using (Db db = new Db())
            {
                PagesDTO dto = db.Pages.Find(id);
                if (dto == null)
                {
                    TempData["EM"] = "This page dose not exist!";
                    return RedirectToAction("Index");
                }
                db.Pages.Remove(dto);
                db.SaveChanges();
            }
            TempData["SM"] = "You have deleted the page";
            return RedirectToAction("Index");
        }

        // Save JS sorting(custom pages sorting)
        [HttpPost]
        public void ReoderPages(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                PagesDTO dto;

                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }

        //GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            SidebarVM model;
            using(Db db = new Db())
            {
                SidebarDTO dto = db.Sidebars.Find(1);
                if (dto == null)
                {
                    TempData["EM"] = "This sidebar dose not exist!";
                    return RedirectToAction("Index");
                }
                model = new SidebarVM(dto);
            }
            return View(model);
        }

        //POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebars.Find(model.Id);
                dto.Body = model.Body;
                db.SaveChanges();
            }
            TempData["SM"] = "You have edited the sidebar";
            return RedirectToAction("EditSidebar");
        }

    }
}