using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Visitors.Domain
{
  public class McIntireTour
  {
    public McIntireTour()
    {
      Tourists = new HashSet<McIntireTourist>();
    }

    public int Id { get; set; }

    [Required]
    public DateTime StartTime { get; set; }
    
    public string Comments { get; set; }

    public int? PrimaryAmbassadorId { get; set; }
    [DisplayName("Primary Amabassador")]
    public virtual Ambassador PrimaryAmbassador { get; set; }

    public int? AssistantAmbassadorId { get; set; }
    [DisplayName("Assistant Amabassador")]
    public virtual Ambassador AssistantAmbassador { get; set; }

    public virtual ICollection<McIntireTourist> Tourists { get; set; }
  }
}
