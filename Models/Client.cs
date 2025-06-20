
namespace JobRecorderNet.Models
{
    public class Client : IValidatableObject
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

        public required ICollection<Address> Addresses { get; set; }

        // Custom validation logic here
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Addresses == null || !Addresses.Any())
            {
                yield return new ValidationResult(
                    "A client must have at least one address.",
                    [nameof(Addresses)]);
            }
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
