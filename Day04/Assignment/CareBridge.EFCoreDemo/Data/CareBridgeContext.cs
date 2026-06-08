using Microsoft.EntityFrameworkCore;          // Contains DbContext, DbSet, ModelBuilder, etc.
using CareBridge.EFCoreDemo.Models;           // Gives access to Patient, Provider, Department, Encounter classes

namespace CareBridge.EFCoreDemo.Data;         // Organizes database-related classes under the Data folder

// DbContext is the heart of EF Core.
// It represents a session/connection with the database.
public class CareBridgeContext : DbContext
{
    // Each DbSet represents a table in SQL Server.
    // EF Core uses these properties to query and save data.

    public DbSet<Patient> Patients { get; set; }       // Maps to Patient table
    public DbSet<Provider> Providers { get; set; }     // Maps to Provider table
    public DbSet<Department> Departments { get; set; } // Maps to Department table
    public DbSet<Encounter> Encounters { get; set; }   // Maps to Encounter table

    // OnConfiguring() is a built-in DbContext method.
    // We override it to tell EF Core:
    // 1. Which database provider to use (SQL Server)
    // 2. Which database to connect to
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(
            "Server=localhost;Database=CareBridgeDB;" +
            "Trusted_Connection=True;TrustServerCertificate=True");
    }

    // OnModelCreating() runs when EF Core builds its internal model.
    // We use it to customize mappings between C# classes and SQL tables.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // By default EF Core may use naming conventions.
        // Here we explicitly map each class to its existing SQL table.

        modelBuilder.Entity<Patient>().ToTable("Patient");         // Patient class -> Patient table
        modelBuilder.Entity<Provider>().ToTable("Provider");       // Provider class -> Provider table
        modelBuilder.Entity<Department>().ToTable("Department");   // Department class -> Department table
        modelBuilder.Entity<Encounter>().ToTable("Encounter");     // Encounter class -> Encounter table
    }
}
