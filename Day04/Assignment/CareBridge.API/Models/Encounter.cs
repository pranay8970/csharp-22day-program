namespace CareBridge.EFCoreDemo.Models;

public class Encounter
{
    public int EncounterId { get; set; }          // EncounterId column (primary key)
    public int PatientId { get; set; }            // PatientId column (FK -> Patient)
    public int ProviderId { get; set; }           // ProviderId column (FK -> Provider)
    public int DepartmentId { get; set; }         // DepartmentId column (FK -> Department)
    public DateTime AdmitDate { get; set; }       // AdmitDate column (DATETIME2)
    public DateTime? DischargeDate { get; set; }  // DischargeDate column (can be empty)
    public string EncounterType { get; set; } = ""; // 'Inpatient' / 'Outpatient' / 'ED'
}
