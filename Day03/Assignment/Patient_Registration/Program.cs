using System;

namespace HospitalRegistration
{
    // Part 1: Patient Class
    public class Patient
    {
        public string PatientID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
    }

    // Registration Manager Class
    public class RegistrationManager
    {
        public static Patient RegisterPatient()
        {
            Patient patient = new Patient();

            // Generate Patient ID
            patient.PatientID = "PAT-" + DateTime.Now.Year + "-001";

            // Name Input Validation
            while (true)
            {
                Console.Write("Enter Patient Name: ");
                string name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    patient.Name = name;
                    break;
                }
                else
                {
                    Console.WriteLine("Error: Name cannot be empty.");
                }
            }

            // Age Validation with Try-Catch
            while (true)
            {
                Console.Write("Enter Age: ");
                try
                {
                    int age = Convert.ToInt32(Console.ReadLine());

                    if (age > 0 && age < 120)
                    {
                        patient.Age = age;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Age must be between 1 and 119.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: Please enter a valid numeric age.");
                }
            }

            // Gender Input
            Console.Write("Enter Gender (Male/Female/Other): ");
            patient.Gender = Console.ReadLine();

            // Phone Number Validation
            while (true)
            {
                Console.Write("Enter Phone Number: ");
                string phone = Console.ReadLine();

                if (phone.Length == 10 && long.TryParse(phone, out _))
                {
                    patient.PhoneNumber = phone;
                    break;
                }
                else
                {
                    Console.WriteLine("Error: Phone number must be exactly 10 digits.");
                }
            }

            // City Input
            Console.Write("Enter City: ");
            patient.City = Console.ReadLine();

            return patient;
        }

        // Display Registration Slip
        public static void DisplaySlip(Patient patient)
        {
            Console.WriteLine("\n[Registration Complete]\n");

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("            PATIENT REGISTRATION SLIP");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Date: " + DateTime.Now.ToShortDateString());
            Console.WriteLine();

            Console.WriteLine("Patient ID: " + patient.PatientID);
            Console.WriteLine("Name:       " + patient.Name);
            Console.WriteLine("Age:        " + patient.Age + " years");
            Console.WriteLine("Gender:     " + patient.Gender);
            Console.WriteLine("Contact:    " + patient.PhoneNumber);
            Console.WriteLine("Location:   " + patient.City);
            Console.WriteLine();

            Console.WriteLine("Instructions:");
            Console.WriteLine("Please proceed to the waiting area.");
            Console.WriteLine("--------------------------------------------------");
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("       HOSPITAL PATIENT REGISTRATION SYSTEM");
            Console.WriteLine("--------------------------------------------------\n");

            Patient patient = RegistrationManager.RegisterPatient();

            RegistrationManager.DisplaySlip(patient);

            Console.ReadLine(); // Pause console
        }
    }
}
