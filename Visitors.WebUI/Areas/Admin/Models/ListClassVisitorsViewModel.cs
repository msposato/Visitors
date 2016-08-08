using System.Collections.Generic;
using Visitors.WebUI.Models;

namespace Visitors.WebUI.Admin.Models
{
  public class ListClassVisitorsViewModel : MultiYearViewModel
  {
    public ListClassVisitorsViewModel()
    {
      UpcomingVisits = new HashSet<ClassVisitorViewModel>();
      PastVisits = new HashSet<ClassVisitorViewModel>();
    }

    public IEnumerable<ClassVisitorViewModel> UpcomingVisits { get; set; }
    public IEnumerable<ClassVisitorViewModel> PastVisits { get; set; }
  }
}