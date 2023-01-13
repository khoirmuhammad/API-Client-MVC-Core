namespace VoxTestApplication.Models
{
    public class ApplicationLog
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
