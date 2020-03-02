using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestNote.DAL.Models;

namespace TestNote.Service.Contracts
{
    public interface IUserSerivce
    {
        Task<UserModel> GetUserByIdAsync(Guid id);
        Task<UserModel> GetUserByIpAsync(string ip);
        Task<List<UserModel>> GetUsersAsync();
        Task UpdateUserAsycn(UserModel userModel);
        Task AddUserAsync(UserModel userModel);
    }
}
