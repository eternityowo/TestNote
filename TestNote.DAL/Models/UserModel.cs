using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestNote.DAL.Models
{
    public class UserModel : BaseModel
    {
        [Required]
        public string UserName { get; set; }
        public string Ip { get; set; }
        public DateTime? BlockDate { get; set; }
    }
}
