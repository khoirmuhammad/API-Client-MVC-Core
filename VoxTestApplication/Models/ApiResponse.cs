namespace VoxTestApplication.Models
{
    public class ApiResponse<T> where T : class
    {
        public string Message { get; set; } = string.Empty;
        public T Errors { get; set; } = default!;
    }
}
