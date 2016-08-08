using System.Collections.Generic;
using Visitors.WebUI.Models;

namespace Visitors.WebUI.Admin.Models
{
  public class ListTouristsViewModel : MultiYearViewModel
  {
    public ListTouristsViewModel()
    {
      Tourists = new HashSet<TouristViewModel>();
    }

    public IEnumerable<TouristViewModel> Tourists { get; set; }
  }
}