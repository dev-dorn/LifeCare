using LifeCare.Personnel.Domain.Common;
using LifeCare.Personnel.Domain.Enum;

namespace LifeCare.Personnel.Domain.Events;

public record PersonnelRegisteredEvent
(
    Guid PersonnelId,
    string FullName,
    PersonnelRole Role
) : DomainEvent;
