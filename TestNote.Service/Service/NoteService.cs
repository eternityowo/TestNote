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
    public class NoteService : BaseService, INoteService
    {
        public NoteService(IUnitOfWork unitOfWork, IEntityGuidConverter entityGuidConverter)
            : base(unitOfWork, entityGuidConverter)
        {
        }

        public List<NoteModel> GetNotes(DateTime startDate, DateTime endDate)
        {
            return UnitOfWork.GetRepository<Notes>().All
                .Where(note => note.CreateDate >= startDate.Date && note.CreateDate <= endDate.Date)
                .Select(note => new
                {
                    note.Id,
                    note.Content,
                    note.CreateDate,
            }).ToList()
            .Select(note => new NoteModel
            {
                Id = EntityGuidConverter.ConvertToPrefixedGuid(typeof(Notes), note.Id),
                Content = note.Content,
                CreateDate = note.CreateDate
            }).ToList();
        }

        public List<NoteModel> GetNotesWithUser(DateTime startDate, DateTime endDate)
        {
            return UnitOfWork.GetRepository<Notes>().AllIncluding(note => note.User)
                .Where(note => note.CreateDate >= startDate.Date && note.CreateDate <= endDate.Date)
                .Select(note => new
                {
                    note.Id,
                    note.Content,
                    note.CreateDate,
                    note.User
                }).ToList()
                .Select(note => new NoteModel
                {
                    Id = EntityGuidConverter.ConvertToPrefixedGuid(typeof(Notes), note.Id),
                    Content = note.Content,
                    CreateDate = note.CreateDate,
                    User = new UserModel() { Ip = note.User.Ip, UserName = note.User.UserName, BlockDate = note.User.BlockDate}
                }).ToList();
        }

        public NoteModel GetNote(string id)
        {
            var note = UnitOfWork.GetRepository<Notes>().GetById(EntityGuidConverter.ConvertToGuid(id));

            return (note == null) ? null : new NoteModel
            {
                Id = EntityGuidConverter.ConvertToPrefixedGuid(typeof(Notes), note.Id),
                Content = note.Content,
                CreateDate = note.CreateDate
            };
        }

        public IServiceResult AddNote(NoteModel note)
        {
            var noteAdded = new Notes
            {
                CreateDate = note.CreateDate,
                Content = note.Content,
                UserId = EntityGuidConverter.ConvertToGuid(note.User.Id),
            };

            UnitOfWork.GetRepository<Notes>().Add(noteAdded);
            UnitOfWork.SaveChanges();

            var resultEntry = GetNote(EntityGuidConverter.ConvertToPrefixedGuid(typeof(Notes), noteAdded.Id));
            return SuccessResult(resultEntry);
        }

        public IServiceResult UpdateNote(NoteModel note)
        {
            var noteRepository = UnitOfWork.GetRepository<Notes>();
            var noteUpdated = noteRepository.GetById(EntityGuidConverter.ConvertToGuid(note.Id));
            if (noteUpdated == null)
                return EntityNotFoundResult<Notes>(note.Id);

            noteUpdated.Content = note.Content;

            noteRepository.Update(noteUpdated);
            UnitOfWork.SaveChanges();

            var resultEntry = GetNote(EntityGuidConverter.ConvertToPrefixedGuid(typeof(Notes), noteUpdated.Id));
            return SuccessResult(resultEntry);
        }

        public IServiceResult DeleteNote(string id)
        {
            var noteRepository = UnitOfWork.GetRepository<Notes>();
            var noteToDelete = noteRepository.GetById(EntityGuidConverter.ConvertToGuid(id));
            if (noteToDelete == null)
                return EntityNotFoundResult<Notes>(id);

            noteRepository.Delete(noteToDelete);
            UnitOfWork.SaveChanges();

            var result = new { deleted = true, ID = id };
            return SuccessResult(result);
        }

        public List<NoteModel> GetNotesByUserId(string id)
        {
            var user = UnitOfWork.GetRepository<Users>().GetById(EntityGuidConverter.ConvertToGuid(id));

            return UnitOfWork.GetRepository<Notes>().All
                .Where(note => note.UserId == user.Id)
                .Select(note => new
                {
                    note.Id,
                    note.Content,
                    note.CreateDate
                }).ToList()
                .Select(note => new NoteModel
                {
                    Id = EntityGuidConverter.ConvertToPrefixedGuid(typeof(Notes), note.Id),
                    Content = note.Content,
                    CreateDate = note.CreateDate,
                    User = new UserModel() { Ip = user.Ip, UserName = user.UserName, BlockDate = user.BlockDate }
                }).ToList();
        }
    }
}
