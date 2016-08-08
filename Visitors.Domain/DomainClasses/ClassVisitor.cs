using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visitors.Domain
{
  public class ClassVisitor
  {
    public ClassVisitor()
    {
      AcademicSubjects = new HashSet<AcademicSubject>();
    }

    public Guid Id { get; set; }

    [Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; }

    [Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; }

    [NotMapped]
    public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string YearInSchool { get; set; }

    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }

    [MaxLength(2)]
    public string State { get; set; }

    public string PostalCode { get; set; }
    public string Country { get; set; }

    public int? AmbassadorId { get; set; }
    public virtual Ambassador Ambassador { get; set; }

    [Required]
    public int PreferredTimeSlotId { get; set; }
    public virtual ClassVisitTimeSlot PreferredTimeSlot { get; set; }

    public int? AlternateTimeSlotId { get; set; }
    public virtual ClassVisitTimeSlot AlternateTimeSlot { get; set; }

    public virtual ICollection<AcademicSubject> AcademicSubjects { get; set; }
  }
}