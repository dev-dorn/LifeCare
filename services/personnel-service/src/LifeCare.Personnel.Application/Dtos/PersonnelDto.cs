namespace LifeCare.Personnel.Application.Dtos
{
    public class PersonnelDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<string> Privileges { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static PersonnelDto FromDomain(LifeCare.Personnel.Domain.Personnel personnel)
        {
            return new PersonnelDto
            {
                Id = personnel.Id,
                FullName = personnel.FullName,
                Email = personnel.Email,
                Role = personnel.Role.ToString(),
                Status = personnel.Status.ToString(),
                Privileges = personnel.Privileges is IEnumerable<string> privs
                    ? privs.ToList()
                    : new List<string>(),
                CreatedAt = personnel.CreatedAt,
                UpdatedAt = personnel.UpdatedAt
            };
        }

}

}