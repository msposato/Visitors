using System.ComponentModel.DataAnnotations;

namespace Visitors.WebUI.Admin.Models
{
  public class AcademicSubjectViewModel
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
  }
}