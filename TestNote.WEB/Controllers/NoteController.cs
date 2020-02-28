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

        [HttpGet]
        public JsonResult Index(DateTime? startDate, DateTime? endDate)
        {
            var timeOffSet = Request.Cookies.FirstOrDefault(c => c.Key == "timezoneoffset").Value;
            var offset = int.Parse(timeOffSet.ToString());


            startDate = startDate.HasValue ? startDate.Value.AddMinutes(offset) : DateTime.UtcNow;
            endDate = endDate.HasValue ? endDate.Value.AddMinutes(offset) : DateTime.UtcNow;
            var notes = _noteService.GetNotesWithUser(startDate.Value, endDate.Value);
            return new JsonResult(notes);
        }

        // GET: Note/Create
        public ActionResult Create()
        {
            var user = _userService.GetUser(HttpContext.Connection.RemoteIpAddress.ToString());
            NoteModel note = new NoteModel() { User = user};
            return PartialView(note);
        }

        // POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteModel note)
        {
            if (ModelState.IsValid)
            {
                note.CreateDate = DateTime.UtcNow;

                return RedirectToAction(nameof(Index));
            }
            else
            {       
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Json("The attached file is not supported", System.Net.Mime.MediaTypeNames.Text.Plain);
            }
        }


        // GET: Note/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Note/Delete/5
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