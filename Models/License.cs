
namespace JobRecorderNet.Models
{
    public enum LicenseType
    {
        Electrical,
        Hardware,
        Data
    }
    public class License
    {
        [Key]
        public int Id { get; set; }

        // Needs a reference to the User model

        [Required, StringLength(50)]
        public required LicenseType Type { get; set; }

        [Required]
        public required DateTime Expiry { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
