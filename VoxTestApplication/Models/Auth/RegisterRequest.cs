using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VoxTestApplication.Models.Auth
{
    public class RegisterRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Passwords must be at least 8 characters, at least one lower case, at least upper one case, at least one number , at least one special character")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Repeat Password is required")]
        [Compare("Password", ErrorMessage = "Password and Repeat Password must match")]
        [DisplayName("Repeat Password")]
        public string RepeatPassword { get; set; } = string.Empty;
    }
}
