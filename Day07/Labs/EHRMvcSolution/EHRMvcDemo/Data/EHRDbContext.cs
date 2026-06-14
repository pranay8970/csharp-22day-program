using EHRMvcDemo.Models;
using EHRMvcDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EHRMvcDemo.Data
{
    // Single source of truth for DB access
    public class EHRDbContext : DbContext
    {
        public EHRDbContext(DbContextOptions<EHRDbContext> options)
            : base(options) { }

        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<LabResult> LabResults => Set<LabResult>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicit schema mapping – enterprise practice
            modelBuilder.Entity<Doctor>().ToTable("Doctors", "Healthcare");
            modelBuilder.Entity<Patient>().ToTable("Patients", "Healthcare");
            modelBuilder.Entity<Appointment>().ToTable("Appointments", "Healthcare");
            modelBuilder.Entity<AuditLog>().ToTable("AuditLog", "Healthcare");
        }
    }
}
