namespace LifeCare.Personnel.Domain.Common;

public abstract record DomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccuredOn { get; set; } = DateTime.UtcNow;
}