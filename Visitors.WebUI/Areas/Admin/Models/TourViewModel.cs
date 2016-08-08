using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Visitors.Domain;

namespace Visitors.WebUI.Admin.Models
{
  public class TourViewModel
  {
    public TourViewModel()
    {
      Tourists = new HashSet<McIntireTourist>();
      ActiveAmbassadors = new Collection<SelectListItem>();
    }

    public int Id { get; set; }

    [Required]
    [DisplayName("Start Time")]
    public DateTime StartTime { get; set; }

    public string Comments { get; set; }

    public int? PrimaryAmbassadorId { get; set; }
    [DisplayName("Primary Ambassador")]
    public Ambassador PrimaryAmbassador { get; set; }

    public int? AssistantAmbassadorId { get; set; }
    [DisplayName("Assistant Ambassador")]
    public Ambassador AssistantAmbassador { get; set; }

    public ICollection<McIntireTourist> Tourists { get; set; }

    public IEnumerable<SelectListItem> ActiveAmbassadors { get; set; }

  }
}