using System;
using System.Collections.Generic;

namespace TestNote.DAL.Entities
{
    public partial class Users
    {
        public Users()
        {
            Notes = new HashSet<Notes>();
        }

        public Guid Id { get; set; }
        public string Ip { get; set; }
        public string UserName { get; set; }
        public DateTime? BlockDate { get; set; }
        public virtual ICollection<Notes> Notes { get; set; }
    }
}
