namespace ApplicationControl.Core.Common;

public interface IEntity<T> 
{
    T Id { get; set; }
    string AddedBy { get; set; }
    DateTime AddedDateTime { get; set; }
    string? UpdatedBy { get; set; }
    DateTime? UpdatedDateTime { get; set; }
    bool IsDeleted { get; set; }
}
