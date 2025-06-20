using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public int SupervisorId { get; set; }
        public required User Supervisor { get; set; }

        public ICollection<User> Technicians { get; set; } = new List<User>();

        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public JobStatus Status { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
