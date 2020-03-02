using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestNote.DAL.Models;

namespace TestNote.Service.Contracts
{
    public interface INoteService
    {
        Task<List<NoteModel>> GetNotesAsync(DateTime startDate, DateTime endDate);
        Task<List<NoteModel>> GetNotesByUserIdAsync(Guid id);
        Task<NoteModel> GetNoteAsync(Guid id);
        Task<IServiceResult> AddNoteAsync(NoteModel note);
        Task<IServiceResult> DeleteNoteAsync(Guid id);
    }
}
