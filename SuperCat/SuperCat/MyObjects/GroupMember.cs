using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCat.MyObjects
{
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }
        public int MemderId { get; set; }
    }
}
