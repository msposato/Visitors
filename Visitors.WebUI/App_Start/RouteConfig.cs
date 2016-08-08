using System.Web.Mvc;
using System.Web.Routing;

namespace Visitors.WebUI
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute("Default",
        "{controller}/{action}/{id}",
        new { controller = "ClassVisitor", action = "Create", id = UrlParameter.Optional },
        new[] { "Visitors.WebUI.Controllers" });
    }
  }
}