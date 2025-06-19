
namespace JobRecorderNet.Models
{
    public class Address
    {
        [Key] 
        public int Id { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }



    }
}
