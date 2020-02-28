using System;
using TestNote.DAL;
using TestNote.DAL.Contracts;
using TestNote.DAL.Entities;
using TestNote.Service.Service;

namespace TestNote.Consol
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitOfWork uof = new UnitOfWork(new NoteDBContext());
            var us = new Users() { Id = Guid.NewGuid(), UserName = "lol" };
            //var uu = uof.GetRepository<Users>().GetById(new Guid("C786ED85-92B2-4863-9BF1-9BE083EDD336"));
            //uof.GetRepository<Users>().Delete(uu);

            //var nn = uof.GetRepository<Notes>().GetById(new Guid("5893914F-E730-4E41-9646-F67C5B91A1A5"));
            //uof.GetRepository<Notes>().Delete(nn);

            //uof.GetRepository<Users>().Add(us);
            //uof.GetRepository<Notes>().Add(new Notes() { Id = Guid.NewGuid(), UserId = us.Id, Content = "RandomMessage111" });
            //uof.GetRepository<Notes>().Add(new Notes() { Id = Guid.NewGuid(), UserId = us.Id, Content = "RandomMessage123" });
            //uof.GetRepository<Notes>().Add(new Notes() { Id = Guid.NewGuid(), UserId = us.Id, Content = "RandomMessage555" });
            //uof.SaveChanges();

            NoteService ns = new NoteService(uof, new EntityGuidConverter());

            var nnn = ns.GetNotes();

            var p = nnn.Count;

            //using (NoteDBContext db = new NoteDBContext())
            //{
            //    db.Notes
            //}
            var gd = Guid.NewGuid().ToString();

            //uof.GetRepository<Notes>().GetById(new Guid("0000-0000-0000-0000-0000000"));
            Console.WriteLine("Hello World!");
        }
    }
}
