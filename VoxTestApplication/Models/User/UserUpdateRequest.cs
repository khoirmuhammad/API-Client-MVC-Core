using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VoxTestApplication.Models.User
{
    public class UserUpdateRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;
    }
}
