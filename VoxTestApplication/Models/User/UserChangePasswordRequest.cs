using System.ComponentModel;

namespace VoxTestApplication.Models.User
{
    public class UserChangePasswordRequest
    {
        public int Id { get; set; }
        [DisplayName("Old Password")]
        public string OldPassword { get; set; } = string.Empty;
        [DisplayName("New Password")]
        public string NewPassword { get; set; } = string.Empty;
        [DisplayName("Repeat Password")]
        public string RepeatPassword { get; set;} = string.Empty;
    }
}
