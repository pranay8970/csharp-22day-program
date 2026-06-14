using System.ComponentModel.DataAnnotations;

namespace EHRMvcDemo.Models
{
    // HIPAA audit trail
    public class AuditLog
    {
        [Key]
        public int AuditId { get; set; }

        public string UserId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;

        public int RecordId { get; set; }
        public int? PatientId { get; set; }

        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }

        public DateTime? AccessDate { get; set; }
        public string? Details { get; set; }
    }
}
