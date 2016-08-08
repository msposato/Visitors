using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Visitors.WebUI.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult About()
    {
      return View();
    }

    public ActionResult Contact()
    {
      return View();
    }
  }
}
