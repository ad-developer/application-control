using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationControl.Core.Common;

namespace ApplicationControl.Core;

[Table("ApplicationControl", Schema = "AppControl")]
public class ApplicationControl : IEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public required string Command { get; set; }
    
    public  string ApplicationName { get; set; } = "Not Available";
    
    [Required]
    public required Guid ApplicationId { get; set; }

    [Required]
    public  string AddedBy { get; set; } = "Not Available";

    [Required]
    public  DateTime AddedDateTime { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedDateTime { get; set; }

    public string? UpdatedBy { get; set; }

    [Required]
    public CommandStatus Status { get; set; } 
    public string? Message { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}
