using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Client.Database.Entities;

[Table("QueuedApplicationJob", Schema = "ApplicationControl")]
public class QueuedApplicationJob : IEntity<Guid>, IJob
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public required string Command { get; set; }
    
    [Required]
    public required Guid ApplicationId { get; set; }

    [Required]
    public  string AddedBy { get; set; } = "Not Available";

    [Required]
    public  DateTime AddedDateTime { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedDateTime { get; set; }

    public string? UpdatedBy { get; set; }

    [Required]
    public JobStatus Status { get; set; } 
    public string? Message { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}

