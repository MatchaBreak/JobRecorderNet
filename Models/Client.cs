
namespace JobRecorderNet.Models
{
    public class Client 
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public required string Name { get; set; }

        [Required, StringLength(255)]
        public required string Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required, StringLength(20)]
        public required string Mobile { get; set; }

        public required ICollection<Address> Addresses { get; set; } = new List<Address>();

        // Temporary property for binding a single address during creation
        public Address? Address { get; set; }
        

        // Navigation property for related jobs
        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
