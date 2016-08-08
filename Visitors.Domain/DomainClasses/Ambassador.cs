using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visitors.Domain
{
  public class Ambassador
  {
    public Ambassador()
    {
      Visitors = new HashSet<ClassVisitor>();
      PrimaryTours = new HashSet<McIntireTour>();
      AssistantTours = new HashSet<McIntireTour>();
    }

    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [NotMapped]
    public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public bool Active { get; set; }

    public string PhoneNumber { get; set; }

    public virtual ICollection<ClassVisitor> Visitors { get; set; }

    public virtual ICollection<McIntireTour> PrimaryTours { get; set; }

    public virtual ICollection<McIntireTour> AssistantTours { get; set; }
  }
}