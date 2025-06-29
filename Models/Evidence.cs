namespace JobRecorderNet.Models
{
    public enum EvidenceType
    {
        Photo,
        Image,
        Document,
        Other
    }

    public class Evidence
    {
        [Key]
        public int Id { get; set; }

        //Foreign key JobId 
        [Required]
        public int JobId { get; set; }
        public required Job Job { get; set; }


        [Required, StringLength(255)]
        public required string Title { get; set; }

        [Required]
        public EvidenceType Type { get; set; }

        // Unique path to local storage
        [Required, StringLength(255)]
        public required string Path { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
