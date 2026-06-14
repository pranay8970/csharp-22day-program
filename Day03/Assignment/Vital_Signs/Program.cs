using System;

namespace VitalSignsMonitoring
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("            VITAL SIGNS MONITOR");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Enter Patient Name: ");
            string patientName = Console.ReadLine();

            double temperature = GetValidTemperature();
            int oxygen = GetValidOxygen();
            int pulse = GetValidPulse();

            Console.WriteLine();
            Console.WriteLine("[Analyzing Data...]");
            Console.WriteLine();

            string status = CheckStatus(temperature, oxygen, pulse);
            string reason = GetReason(temperature, oxygen, pulse);
            string action = GetAction(status);

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("       MEDICAL ASSESSMENT REPORT");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine($"Patient: {patientName}");
            Console.WriteLine();

            Console.WriteLine("Vitals Recorded:");
            Console.WriteLine($"- Temp:   {temperature} C");
            Console.WriteLine($"- Oxygen: {oxygen} %");
            Console.WriteLine($"- Pulse:  {pulse} BPM");
            Console.WriteLine();

            Console.WriteLine($"Status Assessment: {status}");

            if (!string.IsNullOrEmpty(reason))
            {
                Console.WriteLine($"(Reason: {reason})");
            }

            Console.WriteLine();
            Console.WriteLine($"Action: {action}");

            Console.WriteLine("--------------------------------------------------");
        }

        // Method to determine patient status
        static string CheckStatus(double temp, int oxygen, int pulse)
        {
            if (temp > 39.0 || oxygen < 90 || pulse < 50 || pulse > 120)
            {
                return "CRITICAL / EMERGENCY";
            }
            else if (temp > 37.5 || oxygen < 95 || pulse > 100)
            {
                return "OBSERVATION NEEDED";
            }
            else
            {
                return "NORMAL";
            }
        }

        // Method to identify reason
        static string GetReason(double temp, int oxygen, int pulse)
        {
            if (temp > 39.0)
                return "Very High Temperature";

            if (oxygen < 90)
                return "Dangerously Low Oxygen Level";

            if (pulse < 50)
                return "Low Pulse Rate";

            if (pulse > 120)
                return "Very High Pulse Rate";

            if (temp > 37.5)
                return "Elevated Temperature";

            if (oxygen < 95)
                return "Slightly Low Oxygen Level";

            if (pulse > 100)
                return "High Pulse Rate";

            return "";
        }

        // Method to suggest action
        static string GetAction(string status)
        {
            switch (status)
            {
                case "CRITICAL / EMERGENCY":
                    return "Immediate medical attention required.";

                case "OBSERVATION NEEDED":
                    return "Nurse to monitor every hour.";

                default:
                    return "Patient condition is stable.";
            }
        }

        // Temperature Validation
        static double GetValidTemperature()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Temperature (C): ");
                    double temp = Convert.ToDouble(Console.ReadLine());

                    if (temp < 25 || temp > 50)
                    {
                        Console.WriteLine("Temperature must be between 25°C and 50°C.");
                        continue;
                    }

                    return temp;
                }
                catch
                {
                    Console.WriteLine("Invalid input! Please enter a numeric value.");
                }
            }
        }

        // Oxygen Validation
        static int GetValidOxygen()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Oxygen Level (%): ");
                    int oxygen = Convert.ToInt32(Console.ReadLine());

                    if (oxygen < 0 || oxygen > 100)
                    {
                        Console.WriteLine("Oxygen level must be between 0 and 100.");
                        continue;
                    }

                    return oxygen;
                }
                catch
                {
                    Console.WriteLine("Invalid input! Please enter a whole number.");
                }
            }
        }

        // Pulse Validation
        static int GetValidPulse()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Pulse Rate (BPM): ");
                    int pulse = Convert.ToInt32(Console.ReadLine());

                    if (pulse < 20 || pulse > 250)
                    {
                        Console.WriteLine("Pulse rate must be between 20 and 250 BPM.");
                        continue;
                    }

                    return pulse;
                }
                catch
                {
                    Console.WriteLine("Invalid input! Please enter a whole number.");
                }
            }
        }
    }
}