namespace VoxTestApplication.Models.User
{
    public class UserChangePasswordErrorResponse
    {
        public string[] OldPassword { get; set; } = default!;
        public string[] NewPassword { get; set; } = default!;
        public string[] RepeatPassword { get; set;} = default!;
    }
}
