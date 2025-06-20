namespace JobRecorderNet.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required, StringLength(255), EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required, StringLength(20)]
        public string Mobile { get; set; }

        [Required, StringLength(255)]
        public string Address { get; set; }

        [Required]
        public string Role { get; set; }

        // Licenses 
        public ICollection<License> Licenses { get; set; } 

        //For pivot table with Jobs
        public ICollection<Job> SupervisedJobs { get; set; }
        public ICollection<Job> TechnicianJobs { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
