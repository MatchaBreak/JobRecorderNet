namespace JobRecorderNet.Models
{
    public enum UserRole
    {
        Admin,
        Manager,
        Contractor,
        Tradesperson,
        Labourer
    }
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public required string Name { get; set; }

        [Required, StringLength(255), EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required, StringLength(20)]
        public required string Mobile { get; set; }

        public Address? Address { get; set; } 
        

        [Required]
        public required UserRole Role { get; set; }

        // Licenses 
        public ICollection<License>? Licenses { get; set; } 

        //For pivot table with Jobs
        public ICollection<Job>? SupervisedJobs { get; set; }
        public ICollection<Job>? TechnicianJobs { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

    }
}
