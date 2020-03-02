using System;

namespace TestNote.DAL.Entities
{
    public partial class Notes
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? UserId { get; set; }
        public virtual Users User { get; set; }
    }
}
