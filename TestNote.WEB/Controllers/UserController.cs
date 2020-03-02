using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNet.Core;
using TestNote.Service.Contracts;
using TestNote.Service.Service;
using System;

namespace TestNote.WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IUserSerivce _userService;
        public UserController(INoteService noteService, IUserSerivce userSerivce)
        {
            _noteService = noteService;
            _userService = userSerivce;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetList(DataTables.AspNet.Core.IDataTablesRequest request)
        {
            var timeOffSet = Request.Cookies.FirstOrDefault(c => c.Key == "timezoneoffset").Value;
            var offset = int.Parse(timeOffSet.ToString());

            var users = await _userService.GetUsersAsync();

            if (request.Search.Value != null)
            {
                users = users.Where(u => u.Ip.StartsWith(request.Search.Value)).ToList();
            }


            foreach (var u in users)
            {
                u.BlockDate = u.BlockDate?.AddMinutes((-1) * offset);
            }

            return Json(new { draw = request.Draw, recordsFiltered = users.Count, recordsTotal = users.Count, data = users });
        }

        public async Task<JsonResult> ChangeUserStatus()
        {
            var userModel = await _userService.GetUserByIpAsync(HttpContext.Connection.RemoteIpAddress.ToString());
            if (userModel == null)
                return Json(new { data = "null", errorMessage = "Something went wrong, user not found" });

            if(userModel.BlockDate.HasValue)
            {
                userModel.BlockDate = null;
            }
            else
            {
                userModel.BlockDate = DateTime.UtcNow;
            }

            await _userService.UpdateUserAsycn(userModel);

            return Json(new { data = "success" });
        }
    }
}