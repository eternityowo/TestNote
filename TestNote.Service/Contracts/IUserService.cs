using System;
using System.Collections.Generic;
using System.Text;
using TestNote.DAL.Models;

namespace TestNote.Service.Contracts
{
    public interface IUserSerivce
    {
        UserModel GetUser(string ip);
        List<UserModel> GetUsers();
    }
}
