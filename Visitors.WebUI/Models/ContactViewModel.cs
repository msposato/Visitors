using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Visitors.WebUI.Models
{
  public class ContactViewModel
  {
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [DisplayName("First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [DisplayName("Last Name")]
    public string LastName { get; set; }

    [DisplayName("Name")]
    public string FullName { get { return String.Format("{0} {1}", FirstName, LastName); } }

    [Required(ErrorMessage = "An email address is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public virtual string Phone { get; set; }

    public string City { get; set; }

    [MaxLength(2)]
    public string State { get; set; }
    public IEnumerable<SelectListItem> States { get { return WebUIHelper.States; } }
  }
}