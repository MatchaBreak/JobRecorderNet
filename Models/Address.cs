using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobRecorderNet.Models
{
    public class Address
    {
<<<<<<< HEAD
        // Primary Key
        [Key] 
=======
        [Key]
>>>>>>> 0c0f9bb256507bf0208275bf2d781b3754702ae3
        public int Id { get; set; }

        // Optional Client referece
        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        // Optional User reference
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Name
        [Required, StringLength(255)]
        public required string Name { get; set; }
            
        // Street
        [Required, StringLength(255)]
        public required string Street { get; set; }

        // Suburb
        [Required, StringLength(100)] 
        public required string Suburb { get; set; }

        // State
        [Required]
        [RegularExpression("NSW|VIC|QLD|WA|SA|ACT|NT|TAS", ErrorMessage = "Invalid state")]
        public required string State { get; set; }

        // Postcode
        [Required, StringLength(4, MinimumLength = 4)]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Postcode must be 4 digits")]
        public required string Postcose { get; set; }

        // Not nullable by default
        [Required]
        public bool IsMain { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
