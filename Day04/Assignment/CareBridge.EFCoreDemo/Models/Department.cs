namespace CareBridge.EFCoreDemo.Models;

public class Department
{
    public int DepartmentId { get; set; }         // DepartmentId column (primary key)
    public string Name { get; set; } = "";        // Name column (e.g. 'Cardiology')
    public string? Location { get; set; }         // Location column
}
