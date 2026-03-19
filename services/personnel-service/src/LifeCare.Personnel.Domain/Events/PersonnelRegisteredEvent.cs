using LifeCare.Personnel.Domain.Common;
using LifeCare.Personnel.Domain.Enums;

namespace LifeCare.Personnel.Domain.Events;

public record PersonnelRegisteredEvent(
    Guid PersonnelId,
    string FirstName,
    string LastName,
    PersonnelRole Role) : DomainEvent;