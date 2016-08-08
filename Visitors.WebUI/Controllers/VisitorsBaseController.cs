using System.Web.Mvc;
using Visitors.Domain;

namespace Visitors.WebUI.Controllers
{
  public class VisitorsBaseController : Controller
  {
    protected readonly IVisitorRepository VisitorsDb;

    public VisitorsBaseController(IVisitorRepository db)
    {
      VisitorsDb = db;
    }


    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);

      if (disposing && VisitorsDb != null)
      {
        VisitorsDb.Dispose();
      }

      base.Dispose(disposing);
    }
  }
}