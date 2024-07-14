using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperCat.MyObjects
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }
        public string Nikname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RealName { get; set; } = string.Empty;
        public string? Birthday { get; set; } = null;
        public string? Gender { get; set; } = null;
        public string? Email { get; set; } = null;
        public byte[]? Image { get; set; } = null;
        public string GroupsInclude { get; set; } = string.Empty;
        public string GroupsThink { get; set; } = string.Empty;

        public UserInfo()
        {
            
        }
        public UserInfo(string nickname, string password, string realName)
        {
            this.Nikname = nickname;
            this.Password = password;
            this.RealName = realName;
        }
    }
}
