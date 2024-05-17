using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCat.MyObjects
{
    public class ChatInfo
    {
        [Key]
        public int Id { get; set; }
        public int WhoSay { get; set; }
        public string WhenSay { get; set; } = null!;
        public string WhatSay { get; set; } = null!;
        public int? FriendId { get; set; }
        public int? GroupId { get; set; }
    }
}
