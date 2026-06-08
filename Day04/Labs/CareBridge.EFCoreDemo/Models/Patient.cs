namespace CareBridge.EFCoreDemo.Models;

public class Patient
{
    public int PatientId { get; set; }            // PatientId column (primary key)
    public string Mrn { get; set; } = "";         // MRN column (unique per patient)
    public string FullName { get; set; } = "";    // FullName column
    public DateOnly DateOfBirth { get; set; }     // DateOfBirth column (DATE)
    public char Gender { get; set; }              // Gender column ('M','F','O')
    public string? City { get; set; }             // City column (can be empty)
    public bool IsActive { get; set; }            // IsActive column (1/0 -> true/false)
}
