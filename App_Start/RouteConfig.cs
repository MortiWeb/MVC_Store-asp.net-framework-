using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_Store
{
    public class RouteConfig
    {
        private static readonly string[] nameSpacesUIArea = { "MVC_Store.Controllers" };
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Cart", "Cart/{action}/{id}", new { controller = "Cart", action = "CartPage", id = UrlParameter.Optional }, nameSpacesUIArea );
            routes.MapRoute("Category", "Shop/{action}/{slug}/{page}", new { controller = "Shop", action = "Main", slug = UrlParameter.Optional, page = UrlParameter.Optional }, nameSpacesUIArea );
            routes.MapRoute("DetailsProduct", "Shop/DetailsProduct/{slug}", new { controller = "Shop", action = "DetailsProduct", slug = UrlParameter.Optional }, nameSpacesUIArea );
            routes.MapRoute("SideBarPartial", "Pages/SideBarPartial", new {controller = "Pages", action = "SideBarPartial" }, nameSpacesUIArea );
            routes.MapRoute("ServiceMenuPartial", "Pages/ServiceMenuPartial", new { controller = "Pages", action = "ServiceMenuPartial" }, nameSpacesUIArea );
            routes.MapRoute("ServicePage", "{slug}", new { controller = "Pages", action = "ServicePage" }, nameSpacesUIArea );
            routes.MapRoute("MainPage", "", new { controller = "Shop", action = "Main" }, nameSpacesUIArea );
        }
    }
}
