using System.ComponentModel;

namespace VoxTestApplication.Models.Organizer
{
    public class Organizer
    {
        public int Id { get; set; }
        [DisplayName("Organizer Name")]
        public string OrganizerName { get; set; } = string.Empty;
        [DisplayName("Image Location")]
        public string ImageLocation { get; set; } = string.Empty;
    }
}
