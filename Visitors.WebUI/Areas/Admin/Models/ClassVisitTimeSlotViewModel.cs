using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Visitors.Domain;

namespace Visitors.WebUI.Admin.Models
{
  public class ClassVisitTimeSlotViewModel
  {
    public ClassVisitTimeSlotViewModel()
    {
      PrimaryVisitors = new HashSet<ClassVisitor>();
      AlternateVisitors = new HashSet<ClassVisitor>();
    }

    public int Id { get; set; }

    [Required]
    [DisplayName("Starting")]
    public DateTime StartDateTime { get; set; }

    [Required]
    [DisplayName("Ending")]
    public DateTime EndDateTime { get; set; }

    [DisplayName("Seats Available")]
    public int SeatsAvailable { get; set; }

    [DisplayName("Seats Remaining")]
    public int SeatsRemaining { get; set; }

    public string Description
    {
      get
      {
        string description = string.Format("{0} - {1}", StartDateTime.ToString("f"), EndDateTime.ToShortTimeString());
        return description;
      }
    }

    public ICollection<ClassVisitor> PrimaryVisitors { get; set; }

    public ICollection<ClassVisitor> AlternateVisitors { get; set; }
  }
}