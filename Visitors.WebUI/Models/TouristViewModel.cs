using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Visitors.Domain;

namespace Visitors.WebUI.Models
{
  public class TouristViewModel : ContactViewModel
  {
    public TouristViewModel()
    {
      AvailableTours = new HashSet<SelectListItem>();
    }

    [Required(ErrorMessage = "Please select a time and date for your tour")]
    public int TourId { get; set; }

    public int? NumberInParty { get; set; }

    public McIntireTour Tour { get; set; }

    public ICollection<SelectListItem> AvailableTours { get; set; }
  }
}