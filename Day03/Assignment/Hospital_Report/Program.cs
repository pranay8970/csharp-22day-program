using System;
using System.Collections.Generic;

namespace HospitalSummaryReport
{
    // PatientRecord Class
    class PatientRecord
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public double BillAmount { get; set; }
        public string Status { get; set; }

        public PatientRecord(string name, string department, double billAmount, string status)
        {
            Name = name;
            Department = department;
            BillAmount = billAmount;
            Status = status;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Creating List of Patient Records
            List<PatientRecord> patients = new List<PatientRecord>()
            {
                new PatientRecord("John Doe", "General", 500, "Discharged"),
                new PatientRecord("Jane Smith", "Dental", 1200, "Admitted"),
                new PatientRecord("Bob Brown", "General", 400, "Discharged"),
                new PatientRecord("Alice Wilson", "Ortho", 2500, "Admitted"),
                new PatientRecord("Sam Kumar", "Dental", 800, "Discharged"),
                new PatientRecord("David Roy", "Cardiology", 3000, "Admitted")
            };

            // Statistics Variables
            int totalPatients = patients.Count;
            double totalRevenue = 0;

            int generalCount = 0;
            int dentalCount = 0;
            int orthoCount = 0;
            int cardiologyCount = 0;

            // Calculate Statistics using foreach
            foreach (PatientRecord patient in patients)
            {
                totalRevenue += patient.BillAmount;

                switch (patient.Department)
                {
                    case "General":
                        generalCount++;
                        break;

                    case "Dental":
                        dentalCount++;
                        break;

                    case "Ortho":
                        orthoCount++;
                        break;

                    case "Cardiology":
                        cardiologyCount++;
                        break;
                }
            }

            // Display Report
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("       DAILY HOSPITAL ACTIVITY REPORT");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Date: {DateTime.Now.ToShortDateString()}");

            Console.WriteLine("\nPatient List:");

            int serialNo = 1;

            foreach (PatientRecord patient in patients)
            {
                Console.WriteLine(
                    $"{serialNo}. {patient.Name,-15} - {patient.Department,-10} - ₹{patient.BillAmount}"
                );
                serialNo++;
            }

            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("SUMMARY STATISTICS");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine($"Total Patients Visited : {totalPatients}");
            Console.WriteLine($"Total Revenue          : ₹{totalRevenue}");

            Console.WriteLine("\nTraffic by Department:");
            Console.WriteLine($"- General     : {generalCount}");
            Console.WriteLine($"- Dental      : {dentalCount}");
            Console.WriteLine($"- Ortho       : {orthoCount}");
            Console.WriteLine($"- Cardiology  : {cardiologyCount}");

            Console.WriteLine("\nEnd of Report.");
            Console.WriteLine("--------------------------------------------------");

            Console.ReadKey();
        }
    }
}