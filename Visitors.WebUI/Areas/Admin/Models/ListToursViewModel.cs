using System.Collections.Generic;

namespace Visitors.WebUI.Admin.Models
{
  public class ListToursViewModel : MultiYearViewModel
  {
    public ListToursViewModel()
    {
      Tours = new HashSet<TourViewModel>();
    }

    public IEnumerable<TourViewModel> Tours { get; set; }
  }
}