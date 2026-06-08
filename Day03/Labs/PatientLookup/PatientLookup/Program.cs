using System;                       // For Console and parsing
using System.Collections.Generic;   // Required for Dictionary<TKey,TValue>

class Program
{
    static void Main(string[] args)
    {
        // Create Dictionary: key = Patient ID (int), value = Patient details (string)
        // Provides O(1) fast lookup by ID
        Dictionary<int, string> patientRecords = new Dictionary<int, string>();

        // Welcome message
        Console.WriteLine("=== HCA Patient Record Lookup System ===");
        Console.WriteLine("Fast search by Patient ID using Dictionary.\n");

        // Main menu loop
        while (true)
        {
            // Display options
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1: Add new patient record");
            Console.WriteLine("2: Lookup patient by ID");
            Console.WriteLine("3: Update existing record");
            Console.WriteLine("4: Remove patient record");
            Console.WriteLine("5: Exit");
            Console.Write("Enter choice (1-5): ");

            string choice = Console.ReadLine();

            // Option 1: Add new record
            if (choice == "1")
            {
                Console.Write("Enter Patient ID (number): ");
                int id = int.Parse(Console.ReadLine());  // Convert string to int

                // Check if ID already exists to avoid duplicate key error
                if (!patientRecords.ContainsKey(id))
                {
                    Console.Write("Enter patient details (e.g., name, condition): ");
                    string details = Console.ReadLine();

                    // Add the key-value pair
                    patientRecords.Add(id, details);
                    Console.WriteLine($"Patient ID {id} added successfully.\n");
                }
                else
                {
                    Console.WriteLine($"Patient ID {id} already exists. Use Update instead.\n");
                }
            }
            // Option 2: Lookup by ID
            else if (choice == "2")
            {
                Console.Write("Enter Patient ID to search: ");
                int id = int.Parse(Console.ReadLine());

                // Safe way to get value - avoids exception if key not found
                if (patientRecords.TryGetValue(id, out string details))
                {
                    Console.WriteLine($"Patient ID {id}: {details}\n");
                }
                else
                {
                    Console.WriteLine($"Patient ID {id} not found.\n");
                }
            }
            // Option 3: Update existing record
            else if (choice == "3")
            {
                Console.Write("Enter Patient ID to update: ");
                int id = int.Parse(Console.ReadLine());

                // Check if record exists
                if (patientRecords.ContainsKey(id))
                {
                    Console.Write("Enter new details: ");
                    string newDetails = Console.ReadLine();

                    // Update value for existing key
                    patientRecords[id] = newDetails;
                    Console.WriteLine($"Patient ID {id} updated.\n");
                }
                else
                {
                    Console.WriteLine($"Patient ID {id} not found.\n");
                }
            }
            // Option 4: Remove record
            else if (choice == "4")
            {
                Console.Write("Enter Patient ID to remove: ");
                int id = int.Parse(Console.ReadLine());

                // Remove returns true if key was found and removed
                if (patientRecords.Remove(id))
                {
                    Console.WriteLine($"Patient ID {id} removed.\n");
                }
                else
                {
                    Console.WriteLine($"Patient ID {id} not found.\n");
                }
            }
            // Option 5: Exit
            else if (choice == "5")
            {
                Console.WriteLine("Lookup system closed. Goodbye!");
                break;
            }
            // Invalid choice
            else
            {
                Console.WriteLine("Invalid choice. Please enter 1-5.\n");
            }
        }
    }
}
