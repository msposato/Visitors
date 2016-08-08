using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visitors.Domain
{
  public class ClassVisitTimeSlot
  {
    public ClassVisitTimeSlot()
    {
      PrimaryVisitors = new HashSet<ClassVisitor>();
      AlternateVisitors = new HashSet<ClassVisitor>();
    }

    public int Id { get; set; }

    [Required]
    public DateTime StartDateTime { get; set; }

    [Required]
    public DateTime EndDateTime { get; set; }

    [NotMapped]
    public string Description
    {
      get { return String.Format("{0} - {1}", StartDateTime.ToString("f"), EndDateTime.ToShortTimeString()); }
    }

    public int SeatsAvailable { get; set; }

    public int SeatsRemaining { get; set; }

    public virtual ICollection<ClassVisitor> PrimaryVisitors { get; set; }

    public virtual ICollection<ClassVisitor> AlternateVisitors { get; set; }
  }
}