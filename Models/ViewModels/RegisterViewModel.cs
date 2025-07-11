using System.ComponentModel.DataAnnotations;

// These have been separated since adding the Identity package will expect these
namespace JobRecorderNet.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string Name { get; set; } = string.Empty;
    }
}
