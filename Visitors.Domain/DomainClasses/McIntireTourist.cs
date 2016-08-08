using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visitors.Domain
{
  public class McIntireTourist
  {
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

    public string Phone { get; set; }

    public string City { get; set; }

    [MaxLength(2)]
    public string State { get; set; }

    public int? NumberInParty { get; set; }

    public int TourId { get; set; }
    [Required]
    public virtual McIntireTour Tour { get; set; }
  }
}