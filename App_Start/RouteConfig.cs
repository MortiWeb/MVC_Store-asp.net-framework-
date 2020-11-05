using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_Store
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("SideBarPartial", "Pages/SideBarPartial", new {controller = "Pages", action = "SideBarPartial" }, new[] { "MVC_Store.Controllers" });
            routes.MapRoute("ServiceMenuPartial", "Pages/ServiceMenuPartial", new { controller = "Pages", action = "ServiceMenuPartial" }, new[] { "MVC_Store.Controllers" });
            routes.MapRoute("ServicePage", "{slug}", new { controller = "Pages", action = "ServicePage" }, new[] { "MVC_Store.Controllers" });
            routes.MapRoute("MainPage", "", new { controller = "Pages", action = "Main" }, new[] { "MVC_Store.Controllers" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
