using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestNote.DAL.Models;
using TestNote.Service.Contracts;
using TestNote.Service.Service;

namespace TestNote.WEB.Controllers
{
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IUserSerivce _userService;
        public NoteController(INoteService noteService, IUserSerivce userSerivce)
        {
            _noteService = noteService;
            _userService = userSerivce;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetList(DateTime? startDate, DateTime? endDate)
        {
            var user = await _userService.GetUserByIpAsync(HttpContext.Connection.RemoteIpAddress.ToString());
            if (user?.BlockDate.HasValue == true)
            {
                return new JsonResult("U are banned");
            }

            if(user == null)
            {
                user = new UserModel()
                {
                    Id = Guid.NewGuid(),
                    Ip = HttpContext.Connection.RemoteIpAddress.ToString()
                };
                await _userService.AddUserAsync(user);
            }

            var timeOffSet = Request.Cookies.FirstOrDefault(c => c.Key == "timezoneoffset").Value;
            var offset = int.Parse(timeOffSet.ToString());

            startDate = startDate.HasValue ? startDate.Value.AddMinutes(offset) : DateTime.UtcNow;
            endDate = endDate.HasValue ? endDate.Value.AddMinutes(offset) : DateTime.UtcNow;
            var notes = await _noteService.GetNotesAsync(startDate.Value, endDate.Value);
            foreach(var n in notes)
            {
                n.CreateDate = n.CreateDate.Value.AddMinutes((-1) * offset);
            }
            return new JsonResult(notes);
        }

        // GET: Note/Create
        public async Task<ActionResult> Create()
        {
            var user = await _userService.GetUserByIpAsync(HttpContext.Connection.RemoteIpAddress.ToString());
            if (user?.BlockDate.HasValue == true)
            {
                return Content("U are banned");
            }

            user = user ?? new UserModel()
            {
                Id = Guid.NewGuid(),
                Ip = HttpContext.Connection.RemoteIpAddress.ToString()
            };

            NoteModel note = new NoteModel() 
            { 
                UserName = user.UserName, 
                UserId = user.Id 
            };
            return PartialView(note);

        }

        // POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NoteModel note)
        {
            if (ModelState.IsValid)
            {
                note.CreateDate = DateTime.UtcNow;
                note.Id = Guid.NewGuid();

                //var user = await _userService.GetUserByIpAsync(HttpContext.Connection.RemoteIpAddress.ToString());
                //note.UserId = user.Id;

                await _noteService.AddNoteAsync(note);

                var timeOffSet = Request.Cookies.FirstOrDefault(c => c.Key == "timezoneoffset").Value;
                var offset = int.Parse(timeOffSet.ToString());

                note.CreateDate = note.CreateDate.Value.AddMinutes((-1) * offset);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return new JsonResult(note);
            }
            else
            {       
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Note invalid format", System.Net.Mime.MediaTypeNames.Text.Plain);
            }
        }
    }
}