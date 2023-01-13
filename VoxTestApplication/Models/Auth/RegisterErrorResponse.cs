using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VoxTestApplication.Models.Auth
{
    public class RegisterErrorResponse
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string RepeatPassword { get; set; } = string.Empty;
    }
}
