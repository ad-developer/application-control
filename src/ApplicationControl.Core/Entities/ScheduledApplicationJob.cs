using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationControl.Core.Extensions;

namespace ApplicationControl.Core.Entities;

public class ScheduledApplicationJob : IEntity<Guid>, IJob
{
    [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string JobName { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; }

        public bool IsExecuted { get; set; } = false;

        public bool IsRecurring { get; set; } = false;

        public int? IntervalMinutes { get; set; }

        [MaxLength(100)]
        public string? CronExpression { get; set; }

        [MaxLength(255)]
        public string? Command { get; set; }

        public DateTime? LastRunTime { get; set; }

        public bool RunIfMissed { get; set; } = true;

        [Required]
        public Guid ApplicationId { get; set; }

        public DateTime AddedDateTime { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(255)]
        public required string AddedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        [MaxLength(255)]
        public string? UpdatedBy { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;
        public JobStatus Status { get; set; }
}
