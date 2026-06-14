using System.ComponentModel.DataAnnotations;

namespace EHRMvcDemo.Models
{
    // Maps to Healthcare.Doctors
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Specialty { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Not in DB – UI convenience
        public string FullName => FirstName + " " + LastName;
    }
}
