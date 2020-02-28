using System;
using System.Collections.Generic;
using System.Text;
using TestNote.DAL.Models;

namespace TestNote.Service.Contracts
{
    public interface INoteService
    {
        List<NoteModel> GetNotes();
        List<NoteModel> GetNotesWithUser(DateTime startDate, DateTime endDate);
        List<NoteModel> GetNotesByUserId(string id);
        List<NoteModel> GetNotesByDateRange(DateTime startDate, DateTime endDate);
        NoteModel GetNote(string id);
        IServiceResult UpdateNote(NoteModel note);
        IServiceResult DeleteNote(string id);
    }
}
