namespace VoxTestApplication.Models.User
{
    public class UserUpdateErrorResponse
    {
        public string[] FirstName { get; set; } = default!;
        public string[] LastName { get; set; } = default!;
        public string[] Email { get; set; } = default!;
    }
}
