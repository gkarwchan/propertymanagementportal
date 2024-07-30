using Microsoft.EntityFrameworkCore;

namespace EFCoreInMemoryDb;

public class ApiConext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "propertyManagement");
    }
}