using System;

class Appointment
{
    public string PatientName;
    public string Department;
    public string Doctor;
    public string Time;

    // Constructor
    public Appointment(string patientName, string department, string doctor, string time)
    {
        PatientName = patientName;
        Department = department;
        Doctor = doctor;
        Time = time;
    }

    // Method to print ticket
    public void PrintTicket()
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("            APPOINTMENT TICKET");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("Patient:    " + PatientName);
        Console.WriteLine("Department: " + Department);
        Console.WriteLine("Doctor:     " + Doctor);
        Console.WriteLine("Time:       " + Time);
        Console.WriteLine("Status:     Confirmed");
        Console.WriteLine("\nPlease arrive 15 mins before your slot.");
        Console.WriteLine("--------------------------------------------------");
    }
}

class Program
{
    static int GetValidChoice(int min, int max)
    {
        int choice;

        while (true)
        {
            Console.Write("Enter Choice: ");
            string input = Console.ReadLine();

            // Validate input (prevents crash on letters)
            if (int.TryParse(input, out choice))
            {
                if (choice >= min && choice <= max)
                {
                    return choice;
                }
            }

            Console.WriteLine("Invalid input. Please try again.");
        }
    }

    static void Main(string[] args)
    {
        // Data Setup
        string[] departments = { "General Medicine", "Dental", "Orthopedics" };

        string[] generalDoctors = { "Dr. A. Kumar", "Dr. B. Singh" };
        string[] dentalDoctors = { "Dr. C. Roy", "Dr. D. Gupta" };
        string[] orthoDoctors = { "Dr. E. Sharma", "Dr. F. Patel" };

        string[] timeSlots = { "10:00 AM", "11:00 AM", "12:00 PM" };

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("       APPOINTMENT BOOKING SYSTEM");
        Console.WriteLine("--------------------------------------------------");

        // Patient Name
        Console.Write("Enter Patient Name: ");
        string patientName = Console.ReadLine();

        while (true)
        {
            // Department Selection
            Console.WriteLine("\nSelect Department:");
            for (int i = 0; i < departments.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {departments[i]}");
            }

            int deptChoice = GetValidChoice(1, departments.Length);
            string selectedDept = departments[deptChoice - 1];

            // Doctor Selection
            string[] selectedDoctors;

            switch (deptChoice)
            {
                case 1:
                    selectedDoctors = generalDoctors;
                    break;
                case 2:
                    selectedDoctors = dentalDoctors;
                    break;
                case 3:
                    selectedDoctors = orthoDoctors;
                    break;
                default:
                    selectedDoctors = generalDoctors;
                    break;
            }

            Console.WriteLine("\nSelect Doctor:");
            for (int i = 0; i < selectedDoctors.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {selectedDoctors[i]}");
            }

            int docChoice = GetValidChoice(1, selectedDoctors.Length);
            string selectedDoctor = selectedDoctors[docChoice - 1];

            // Time Slot Selection
            Console.WriteLine("\nSelect Time Slot:");
            for (int i = 0; i < timeSlots.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {timeSlots[i]}");
            }

            int timeChoice = GetValidChoice(1, timeSlots.Length);
            string selectedTime = timeSlots[timeChoice - 1];

            // Confirmation
            Console.WriteLine("\n[Booking Confirmed]\n");

            // Create Appointment object
            Appointment appointment = new Appointment(
                patientName,
                selectedDept,
                selectedDoctor,
                selectedTime
            );

            // Print Ticket
            appointment.PrintTicket();

            break; // Exit after booking
        }
    }
}