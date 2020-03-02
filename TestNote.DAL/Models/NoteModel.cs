using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestNote.DAL.Models
{
    public class NoteModel : BaseModel
    {
        [Required]
        public string Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
    }
}
