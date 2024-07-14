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
        public string WhenSay { get; set; } = string.Empty;
        public string WhatSay { get; set; } = string.Empty;
        public int? FriendId { get; set; } = null!;
        public int? GroupInfoId { get; set; } = null!;
        public ChatInfo()
        {
            
        }
        public ChatInfo(int who, string when, string what, int? friend, int? group)
        {
            this.WhoSay = who;
            this.WhenSay = when;
            this.WhatSay = what; 
            this.FriendId = friend;
            this.GroupInfoId = group;
        }
    }
}
