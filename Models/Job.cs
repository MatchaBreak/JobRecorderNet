namespace JobRecorderNet.Models
{
    public enum JobType
    {
        Electrical,
        Hardware,
        Data
    }

    public enum JobStatus
    {
        Pending,
        Active,
        Completed
    }

    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public required string JobNo { get; set; }

        [Required]
        public JobType Type { get; set; }

        [Required]
        public int ClientId { get; set; }
        public required Client Client { get; set; }

        // Job has 1 supervisor and can have multiple technicians
        [Required]
        public int SupervisorId { get; set; }
        public required User Supervisor { get; set; }

        public ICollection<User> Technicians { get; set; } = new List<User>();

        [Required]
        public int AddressId { get; set; }
        public required Address Address { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public JobStatus Status { get; set; }

        // For storing evidence 
        public ICollection<Evidence> Evidences { get; set; } = new List<Evidence>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
