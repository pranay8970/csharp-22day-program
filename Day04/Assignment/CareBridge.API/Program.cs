using CareBridge.EFCoreDemo.Models.Generated;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register EF Core DbContext.
// ASP.NET Core will automatically create and inject it when needed.
builder.Services.AddDbContext<CareBridgeScaffoldContext>();

// Add Swagger support.
// Swagger gives us a testing screen for APIs.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow Vue.js running on another port
// to call this API from the browser.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Enable Swagger.
app.UseSwagger();
app.UseSwaggerUI();

// Enable CORS.
app.UseCors();

// Simple health-check endpoint.
app.MapGet("/", () =>
{
    return "CareBridge API is running";
});

// Return first 20 patients.
// EF Core converts this LINQ query into SQL.
app.MapGet("/api/patients",
    (CareBridgeScaffoldContext db) =>
    {
        return db.Patients

                 // Select only columns we need.
                 .Select(p => new
                 {
                     p.PatientId,
                     p.FullName,
                     p.City
                 })

                 // Return only first 20 rows.
                 .Take(20)

                 // Execute query.
                 .ToList();
    });
// ✅ Search endpoint with optional filters
app.MapGet("/api/patients/search",
    (CareBridgeScaffoldContext db, string? city, bool? activeOnly) =>
    {
        var query = db.Patients.AsQueryable();

        // ✅ Filter by city (only if provided)
        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(p => p.City == city);
        }

        // ✅ Filter by active status (only if true)
        if (activeOnly.HasValue && activeOnly.Value)
        {
            query = query.Where(p => p.IsActive == true);
        }

        // ✅ Shape + sort results
        var result = query
            .OrderBy(p => p.FullName)
            .Select(p => new
            {
                patientId = p.PatientId,
                fullName = p.FullName,
                city = p.City,
                isActive = p.IsActive
            })
            .ToList();

        return result;

    });
// ✅ Department Load Analytics Endpoint
app.MapGet("/api/analytics/department-load",
    (CareBridgeScaffoldContext db) =>
    {
        var cutoffDate = DateTime.Now.AddDays(-60);
        var today = DateTime.Now;
        var result = db.Encounters
            .Where(e => e.AdmitDate>=cutoffDate)
            .Join(db.Departments,
                e => e.DepartmentId,
                d => d.DepartmentId,
                (e, d) => new { e, d })
            .GroupBy(x => x.d.Name)
            .Select(g => new
            {
                departmentName = g.Key,

                inpatient = g.Count(x => x.e.EncounterType == "Inpatient"),
                outpatient = g.Count(x => x.e.EncounterType == "Outpatient"),
                ed = g.Count(x => x.e.EncounterType == "ED"),

                // ✅ total = all encounters in that department
                total = g.Count()
            })
            .OrderByDescending(x => x.total)
            .ToList();

        return result;
    });

app.Run();
