using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCat.MyObjects
{
    public class UserInfo
    {
        public int ID { get; set; }
        public string Nikname { get; set; } = string.Empty;
        public string Password { get; set; }= string.Empty;
        public string RealName { get; set; }= string.Empty;
        public string? Birthday { get; set; }= null;
        public string? Gender { get; set; } = null;
        public string? Email { get; set; }= null;
        public byte[]? Image { get; set; }= null;
        public UserInfo()
        {
            
        }
        public UserInfo(string nikname, string password, string realname)
        {
            Nikname = nikname;
            Password = password;
            RealName = realname;
        }
    }
}
