using System;

namespace Billing_Module
{
    class Bill
    {
        public const decimal ConsultationFee = 500m;
        public const decimal BloodTestFee = 200m;
        public const decimal XRayFee = 1000m;
        public const decimal AdmissionFee = 2000m;

        public string PatientName { get; set; }
        public int Age { get; set; }

        public decimal BaseAmount { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal TaxAmount { get; private set; }
        public decimal NetAmount { get; private set; }

        private bool consultationAdded = false;

        public void AddService(int choice)
        {
            switch (choice)
            {
                case 1:
                    BaseAmount += ConsultationFee;
                    consultationAdded = true;
                    Console.WriteLine("[Added Consultation]");
                    break;

                case 2:
                    BaseAmount += BloodTestFee;
                    Console.WriteLine("[Added Blood Test]");
                    break;

                case 3:
                    BaseAmount += XRayFee;
                    Console.WriteLine("[Added X-Ray]");
                    break;

                case 4:
                    BaseAmount += AdmissionFee;
                    Console.WriteLine("[Added Admission]");
                    break;

                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            }
        }

        public void CalculateBill()
        {
            DiscountAmount = 0;

            if (Age > 60)
            {
                DiscountAmount = BaseAmount * 0.20m;
            }
            else if (Age < 10 && consultationAdded)
            {
                DiscountAmount = ConsultationFee * 0.50m;
            }

            decimal amountAfterDiscount = BaseAmount - DiscountAmount;

            TaxAmount = amountAfterDiscount * 0.05m;

            NetAmount = amountAfterDiscount + TaxAmount;
        }

        public string GetCategory()
        {
            if (Age > 60)
                return "Senior Citizen";

            if (Age < 10)
                return "Child";

            return "Regular Patient";
        }

        public void PrintInvoice()
        {
            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("FINAL BILL INVOICE");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine($"Patient: {PatientName} ({GetCategory()})");

            Console.WriteLine($"\nBase Amount:      {BaseAmount:F2}");
            Console.WriteLine($"Discount:        -{DiscountAmount:F2}");
            Console.WriteLine($"Tax (5%):        +{TaxAmount:F2}");

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"TOTAL PAYABLE:    {NetAmount:F2}");
            Console.WriteLine("--------------------------------------------------");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bill bill = new Bill();

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("HOSPITAL BILLING CALCULATOR");
            Console.WriteLine("--------------------------------------------------");

            Console.Write("Patient Name: ");
            bill.PatientName = Console.ReadLine();

            Console.Write("Patient Age: ");

            int age;

            while (!int.TryParse(Console.ReadLine(), out age))
            {
                Console.Write("Enter a valid age: ");
            }

            bill.Age = age;

            while (true)
            {
                Console.WriteLine("\nAdd Services:");
                Console.WriteLine("1. Consultation (500)");
                Console.WriteLine("2. Blood Test (200)");
                Console.WriteLine("3. X-Ray (1000)");
                Console.WriteLine("4. Admission (2000)");
                Console.WriteLine("5. Done");

                Console.Write("Choice: ");

                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid Input");
                    continue;
                }

                if (choice == 5)
                    break;

                bill.AddService(choice);
            }

            Console.WriteLine("\n[Calculating Bill...]");

            bill.CalculateBill();
            bill.PrintInvoice();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}