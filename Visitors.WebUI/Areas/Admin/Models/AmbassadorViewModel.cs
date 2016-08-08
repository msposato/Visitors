using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Visitors.Domain;

namespace Visitors.WebUI.Admin.Models
{
  public class AmbassadorViewModel
  {
    public AmbassadorViewModel()
    {
      Visitors = new HashSet<ClassVisitor>();
      PrimaryTours = new HashSet<McIntireTour>();
      AssistantTours = new HashSet<McIntireTour>();
    }

    public int Id { get; set; }

    [Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; }

    [Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; }

    [DisplayName("Name")]
    public string FullName
    {
      get { return string.Format("{0} {1}", FirstName, LastName); }      
    }

    [Required]
    [DisplayName("User Name")]
    public string UserName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public bool Active { get; set; }

    [DisplayName("Phone Number")]
    public string PhoneNumber { get; set; }

    public ICollection<ClassVisitor> Visitors { get; set; }

    public ICollection<McIntireTour> PrimaryTours { get; set; }

    public ICollection<McIntireTour> AssistantTours { get; set; }

    public ICollection<McIntireTour> AllTours
    {
      get
      {
        List<McIntireTour> tours = new List<McIntireTour>();
        tours.AddRange(PrimaryTours);
        tours.AddRange(AssistantTours);
        return tours;
      }
    }
  }
}