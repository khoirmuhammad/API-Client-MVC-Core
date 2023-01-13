using AutoMapper;
using VoxTestApplication.Models.Organizer;

namespace VoxTestApplication.ProfileMappers.Organizer
{
    public class OrganizerMapper : Profile
    {
        public OrganizerMapper()
        {
            CreateMap<Models.Organizer.Organizer, OrganizerCreateRequest>().ReverseMap();
        }
    }
}
