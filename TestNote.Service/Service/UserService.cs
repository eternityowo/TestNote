using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNote.DAL;
using TestNote.DAL.Contracts;
using TestNote.DAL.Entities;
using TestNote.DAL.Models;
using TestNote.Service.Contracts;

namespace TestNote.Service.Service
{
    public class UserService : BaseService, IUserSerivce
    {
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
        {
        }

        public Task<UserModel> GetUserByIdAsync(Guid id)
        {
            return UnitOfWork.GetRepository<Users>().All()
                .Where(user => user.Id == id)
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public Task<UserModel> GetUserByIpAsync(string ip)
        {
            return UnitOfWork.GetRepository<Users>().All()
                .Where(user => user.Ip == ip)
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public Task<List<UserModel>> GetUsersAsync()
        {
            return UnitOfWork.GetRepository<Users>().All()
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task UpdateUserAsycn(UserModel userModel)
        {
            var user = _mapper.Map<Users>(userModel);
            UnitOfWork.GetRepository<Users>().Update(user);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task AddUserAsync(UserModel userModel)
        {
            var user = _mapper.Map<Users>(userModel);
            await UnitOfWork.GetRepository<Users>().InsertAsync(user);
            await UnitOfWork.SaveChangesAsync();
        }

    }
}
