using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LifeCare.Personnel.Infrastructure.Persistence;

public class PersonnelDbcontextFactory: IDesignTimeDbContextFactory<PersonnelDbContext>

{
    public PersonnelDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PersonnelDbContext>();
        optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5433;Database=personnel_db;Username=Admin;Password=Admin123");
            return new PersonnelDbContext(optionsBuilder.Options);

            
    }
}