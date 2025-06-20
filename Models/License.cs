
namespace JobRecorderNet.Models
{
    public class License
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public required string Type { get; set; }

        [Required]
        public required DateTime Expiry { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
