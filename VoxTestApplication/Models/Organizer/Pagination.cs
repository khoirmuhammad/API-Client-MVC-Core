namespace VoxTestApplication.Models.Organizer
{
    public class Pagination
    {
        public int Total { get; set; }
        public int Count { get; set; }
        public int Per_Page { get; set; }
        public int Current_Page { get; set; }
        public int Total_Pages { get; set; }
        public Link Links { get; set; } = default!;
    }
}
