using System.Web.Mvc;
using Visitors.Domain;

namespace Visitors.WebUI.Admin.Controllers
{
  [Authorize(Roles = "Software-Development, McIntire-Ambassadors")]
  public class AdminBaseController : Controller
  {
    protected readonly IVisitorRepository VisitorsDb;

    public AdminBaseController(IVisitorRepository visitorsDb)
    {
      VisitorsDb = visitorsDb;
    }
  }
}
