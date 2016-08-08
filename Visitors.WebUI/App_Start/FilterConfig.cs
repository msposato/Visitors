using System.Web;
using System.Web.Mvc;

namespace Visitors.WebUI
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new ElmahHandledErrorLoggerFilter());
      filters.Add(new HandleErrorAttribute());
    }
  }
}