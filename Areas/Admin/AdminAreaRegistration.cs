using System.Web.Mvc;

namespace MVC_Store.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        private static readonly string[] nameSpacesAdminArea = { "MVC_Store.Areas.Admin.Controllers" };
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_Pages",
                "Admin/Pages/{action}/{id}",
                new { controller = "Pages", action = "Index", id = UrlParameter.Optional }, nameSpacesAdminArea
            );
            context.MapRoute(
                "Admin_Shop",
                "Admin/Shop/{action}/{id}",
                new { controller = "Shop", action = "Products", id = UrlParameter.Optional }, nameSpacesAdminArea
            );
        }
    }
}