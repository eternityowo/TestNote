using AutoMapper;
using TestNote.DAL.Entities;
using TestNote.DAL.Models;

namespace TestNote.WEB.Mapping
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<Notes, NoteModel>(MemberList.None);
            CreateMap<Users, UserModel>(MemberList.None);
        }
    }
}
