namespace JobRecorderNet.Models
{
    public class Address
    {
        // Primary Key
        [Key] 
        public int Id { get; set; }

        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        public int? UserId { get; set; }
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
        public required string Postcode { get; set; }

        // Not nullable by default
        [Required]
        public bool IsMain { get; set; }

        public override string ToString()
        {
            return $"{Name}, {Street} {Suburb}, {State}, {Postcode}";
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
