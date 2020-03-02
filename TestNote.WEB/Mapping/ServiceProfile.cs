using AutoMapper;
using TestNote.DAL.Entities;
using TestNote.DAL.Models;

namespace TestNote.WEB.Mapping
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<NoteModel, Notes>(MemberList.None);
            CreateMap<UserModel, Users>(MemberList.None);
        }
    }
}
