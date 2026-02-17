namespace LifeCare.API.Controllers.Requests;

public class UpdateStatusRequest
{
    public string NewStatus { get; set; } = null!;
    public string ChangedBy { get; set; } = null!;
    public DateTime ChangedAt { get; set; }
    public string? Notes { get; set; }
    
}