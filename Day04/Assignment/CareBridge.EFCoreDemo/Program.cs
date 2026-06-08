
using CareBridge.EFCoreDemo.Models.Generated;

// Create a DbContext object.
// DbContext represents a session/connection with the database.
// Through this object we can query and modify data in CareBridgeDB.
using var db = new CareBridgeScaffoldContext();

while (true)
{
    Console.Clear();

    Console.WriteLine("=================================");
    Console.WriteLine("       LINQ QUERY DEMO");
    Console.WriteLine("=================================");
    Console.WriteLine("1. Get All Patients");
    Console.WriteLine("2. Active Patients");
    Console.WriteLine("3. Patients From Pune");
    Console.WriteLine("4. Recent Encounters");
    Console.WriteLine("5. Denied Claims");
    Console.WriteLine("6. Join Patient + Encounter");
    Console.WriteLine("7. Encounters By Department");
    Console.WriteLine("8. Exit");
    Console.WriteLine();

    Console.Write("Choose an option (1-8): ");
    string? choice = Console.ReadLine();

    Console.WriteLine();

    switch (choice)
    {
        case "1":

            Console.WriteLine("===== QUERY 1 - ALL PATIENTS =====");

            // db.Patients represents the Patient table.
            // ToList() executes the query and fetches all rows.
            //
            // SQL Equivalent:
            // SELECT * FROM Patient

            var all = db.Patients.ToList();

            Console.WriteLine($"Total patients: {all.Count}");

            break;

        case "2":

            Console.WriteLine("===== QUERY 2 - ACTIVE PATIENTS =====");

            // Where() is the LINQ equivalent of SQL WHERE.
            //
            // p => p.IsActive means:
            // "For each patient p, keep only rows
            // where IsActive is true."
            //
            // SQL Equivalent:
            // SELECT * FROM Patient
            // WHERE IsActive = 1

            var active = db.Patients
                           .Where(p => p.IsActive)
                           .ToList();

            Console.WriteLine($"Active patients: {active.Count}");

            break;

        case "3":

            Console.WriteLine("===== QUERY 3 - PATIENTS FROM PUNE =====");

            // Step 1: Filter only Pune patients.
            // Step 2: Sort by FullName (A-Z).
            // Step 3: Execute the query.
            //
            // SQL Equivalent:
            // SELECT *
            // FROM Patient
            // WHERE City = 'Pune'
            // ORDER BY FullName

            var punePatients = db.Patients
                                 .Where(p => p.City == "Pune")
                                 .OrderBy(p => p.FullName)
                                 .ToList();

            foreach (var patient in punePatients)
            {
                Console.WriteLine(patient.FullName);
            }

            Console.WriteLine($"\nTotal patients from Pune: {punePatients.Count}");

            break;

        case "4":

            Console.WriteLine("===== QUERY 4 - RECENT ENCOUNTERS =====");

            // Create a date representing 30 days ago.
            var cutoff = DateTime.Now.AddDays(-30);

            // Keep only encounters whose AdmitDate
            // is within the last 30 days.
            //
            // OrderByDescending() sorts newest first.
            //
            // SQL Equivalent:
            // SELECT *
            // FROM Encounter
            // WHERE AdmitDate >= cutoff
            // ORDER BY AdmitDate DESC

            var recent = db.Encounters
                           .Where(e => e.AdmitDate >= cutoff)
                           .OrderByDescending(e => e.AdmitDate)
                          
                           .ToList();

            Console.WriteLine($"Encounters in last 30 days: {recent.Count}");

            break;

        case "5":

            Console.WriteLine("===== QUERY 5 - DENIED CLAIMS =====");

            // Filter only claims whose status is Denied.
            //
            // SQL Equivalent:
            // SELECT *
            // FROM Claim
            // WHERE Status = 'Denied'

            var denied = db.Claims
                           .Where(c => c.Status == "Denied")
                           .ToList();

            // Sum() adds all BilledAmount values together.
            //
            // SQL Equivalent:
            // SELECT SUM(BilledAmount)
            // FROM Claim
            // WHERE Status = 'Denied'

            decimal lost = denied.Sum(c => c.BilledAmount);

            Console.WriteLine(
                $"Denied claims: {denied.Count}, billed total: {lost:C}");

            break;

        case "6":

            Console.WriteLine("===== QUERY 6 - JOIN PATIENT + ENCOUNTER =====");

            // JOIN combines rows from multiple tables.
            //
            // Here we join:
            // Encounter table
            // +
            // Patient table
            //
            // Matching condition:
            // Encounter.PatientId = Patient.PatientId
            //
            // SQL Equivalent:
            //
            // SELECT p.FullName,
            //        e.EncounterType,
            //        e.AdmitDate
            // FROM Encounter e
            // JOIN Patient p
            //      ON e.PatientId = p.PatientId

            var joined =
            (
                from e in db.Encounters

                join pat in db.Patients
                    on e.PatientId equals pat.PatientId

                // Create a smaller result object.
                // Similar to choosing specific columns in SQL.
                select new
                {
                    pat.FullName,
                    e.EncounterType,
                    e.AdmitDate
                    
                }
            )
            .OrderByDescending(e => e.AdmitDate)
            .Take(10) // Similar to SQL TOP 10
            .ToList();

            foreach (var row in joined)
            {
                Console.WriteLine(
                    $"{row.FullName} - {row.EncounterType} on {row.AdmitDate:d}");
            }

            break;

        case "7":

            Console.WriteLine("===== QUERY 7 - ENCOUNTERS BY DEPARTMENT =====");

            // GroupBy() is the LINQ equivalent of SQL GROUP BY.
            //
            // SQL Equivalent:
            //
            // SELECT DepartmentId,
            //        COUNT(*)
            // FROM Encounter
            // GROUP BY DepartmentId

            var byDept = db.Encounters

                           // Create one group per department
                           .GroupBy(e => e.DepartmentId)

                           // Create a result object for each group
                           .Select(g => new
                           {
                               DepartmentId = g.Key,

                               // Count rows in this group
                               Count = g.Count()
                           })

                           // Highest count first
                           .OrderByDescending(x => x.Count)

                           .ToList();

            foreach (var row in byDept)
            {
                Console.WriteLine(
                    $"Department {row.DepartmentId}: {row.Count} encounters");
            }

            break;

        case "8":

            Console.WriteLine("Goodbye!");
            return;

        default:

            Console.WriteLine("Please enter a number from 1 to 8.");
            break;
    }

    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}





