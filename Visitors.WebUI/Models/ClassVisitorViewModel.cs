using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Visitors.Domain;

namespace Visitors.WebUI.Models
{
  public class ClassVisitorViewModel : ContactViewModel
  {
    private readonly ICollection<SelectListItem> _schoolYears;
 
    public ClassVisitorViewModel()
    {
      AcademicSubjects = new HashSet<CheckBoxItem>();
      AvailableTimeSlots = new HashSet<SelectListItem>();
      AlternateTimeSlots = new HashSet<SelectListItem>();
      Ambassadors = new HashSet<SelectListItem>();

      _schoolYears = new HashSet<SelectListItem>
        {
          new SelectListItem {Selected = false, Text = "UVA First Year", Value = "UVA First Year"},
          new SelectListItem {Selected = false, Text = "UVA Second Year", Value = "UVA Second Year"},
          new SelectListItem {Selected = false, Text = "Other College Freshman", Value = "Other College Freshman"},
          new SelectListItem {Selected = false, Text = "Other College Sophomore", Value = "Other College Sophomore"},
          new SelectListItem {Selected = false, Text = "Other College Junior", Value = "Other College Junior"},
          new SelectListItem {Selected = false, Text = "High School Senior", Value = "High School Senior"},
          new SelectListItem {Selected = false, Text = "High School Junior", Value = "High School Junior"},
          new SelectListItem {Selected = false, Text = "Other", Value = "Other"}
        };

    }

    [Required(ErrorMessage = "Please indicate your current school year.")]
    [DisplayName("Year In School")]
    public string YearInSchool { get; set; }

    public string Address1 { get; set; }
    public string Address2 { get; set; }

    [DisplayName("Postal Code")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    public override string Phone { get; set; }

    public string Country { get; set; }

    public int? AmbassadorId { get; set; }
    public virtual Ambassador Ambassador { get; set; }
    public IEnumerable<SelectListItem> Ambassadors { get; set; }

    [Required(ErrorMessage = "Please select a time for your visit.")]
    [DisplayName("Prefered Time Slot")]
    public int PreferedTimeSlotId { get; set; }

    public virtual ClassVisitTimeSlot PreferedTimeSlot { get; set; }

    public int? AlternateTimeSlotId { get; set; }
    [DisplayName("Alternate Time Slot")]
    public virtual ClassVisitTimeSlot AlternateTimeSlot { get; set; }

    public IEnumerable<CheckBoxItem> AcademicSubjects { get; set; }

    public ICollection<SelectListItem> AvailableTimeSlots { get; set; }

    public ICollection<SelectListItem> AlternateTimeSlots { get; set; }

    public ICollection<SelectListItem> SchoolYears { get { return _schoolYears; } }
  }
}