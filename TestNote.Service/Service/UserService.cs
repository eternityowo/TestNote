using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestNote.DAL;
using TestNote.DAL.Contracts;
using TestNote.DAL.Entities;
using TestNote.DAL.Models;
using TestNote.Service.Contracts;

namespace TestNote.Service.Service
{
    public class UserService : BaseService, IUserSerivce
    {
        public UserService(IUnitOfWork unitOfWork, IEntityGuidConverter entityGuidConverter)
        : base(unitOfWork, entityGuidConverter)
        {
        }

        public UserModel GetUser(string ip)
        {
            return UnitOfWork.GetRepository<Users>().All.Where(user => user.Ip == ip)
            .Select(user => new
            {
                user.Id,
                user.Ip,
                user.UserName,
                user.BlockDate
            }).ToList().Select(user => new UserModel
            {
                Id = EntityGuidConverter.ConvertToPrefixedGuid(typeof(Notes), user.Id),
                Ip = user.Ip,
                UserName = user.UserName
            }).FirstOrDefault();
        }

        public List<UserModel> GetUsers()
        {
            return UnitOfWork.GetRepository<Users>().Get(out int total, "Id", 0, 10, true).Select(user => new
            {
                user.Id,
                user.Ip,
                user.UserName,
                user.BlockDate
            }).ToList().Select(user => new UserModel
            {
                Id = EntityGuidConverter.ConvertToPrefixedGuid(typeof(Notes), user.Id),
                Ip = user.Ip,
                UserName = user.UserName,
                BlockDate = user.BlockDate
            }).ToList();
        }
    }
}
