using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationControl.Core.Extensions;


namespace ApplicationControl.Core.Entities;

[Table("ApplicationJob", Schema = "AppControl")]
public class ApplicationJob : IEntity<Guid>
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

    public string? Description { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}
