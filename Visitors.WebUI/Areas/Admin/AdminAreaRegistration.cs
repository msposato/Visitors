using System.Web.Mvc;

namespace Visitors.WebUI.Admin
{
  public class AdminAreaRegistration : AreaRegistration
  {
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
          "Admin_default",
          "Admin/{controller}/{action}/{id}",
          new { controller = "ClassVisitor", action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}
