using AutoMapper;
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
using AutoMapper.QueryableExtensions;

namespace TestNote.Service.Service
{
    public class NoteService : BaseService, INoteService
    {
        public NoteService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }
        public Task<NoteModel> GetNoteAsync(Guid id)
        {
            return UnitOfWork.GetRepository<Notes>().All()
                .Where(note => note.Id == id)
                .ProjectTo<NoteModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public Task<List<NoteModel>> GetNotesAsync(DateTime startDate, DateTime endDate)
        {
            return UnitOfWork.GetRepository<Notes>().All()
                .Where(note => note.CreateDate >= startDate.Date && note.CreateDate <= endDate.Date)
                .ProjectTo<NoteModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<List<NoteModel>> GetNotesByUserIdAsync(Guid id)
        {
            return UnitOfWork.GetRepository<Notes>().All()
                .Where(note => note.UserId.Value == id)
                .ProjectTo<NoteModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IServiceResult> AddNoteAsync(NoteModel noteModel)
        {
            var note = _mapper.Map<Notes>(noteModel);
            //note.User = null;
            await UnitOfWork.GetRepository<Notes>().InsertAsync(note);
            await UnitOfWork.SaveChangesAsync();

            var resultEntry = await UnitOfWork.GetRepository<Notes>().AllIncluding(_ => _.User)
                .Where(_ => _.Id == note.Id)
                .FirstOrDefaultAsync();
            return SuccessResult(_mapper.Map<NoteModel>(resultEntry));
        }


        public async Task<IServiceResult> DeleteNoteAsync(Guid id)
        {
            var noteToDelete = UnitOfWork.GetRepository<Notes>().GetByIdAsync(id);
            if (noteToDelete == null)
                return EntityNotFoundResult<Notes>(id.ToString());

            UnitOfWork.GetRepository<Notes>().Delete(noteToDelete);
            await UnitOfWork.SaveChangesAsync();

            var result = new { deleted = true, ID = id };
            return SuccessResult(result);
        }
    }
}
