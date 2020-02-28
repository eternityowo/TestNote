using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNet.Core;
using TestNote.Service.Contracts;
using TestNote.Service.Service;

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

        public JsonResult GetList(DataTables.AspNet.Core.IDataTablesRequest request)
        {
            var users = _userService.GetUsers()/*.Where(user => user.Ip.StartsWith(request.Search.Value)).ToList()*/;

            return Json(new { draw = request.Draw, recordsFiltered = users.Count, recordsTotal = users.Count, data = users });
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}