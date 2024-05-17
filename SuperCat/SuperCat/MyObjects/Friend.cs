using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCat.MyObjects
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }
        public int FfriendId { get; set; }
        public int SfriendId { get; set; }
        public int? WhoThink { get; set; } = null!;
        public Friend()
        {
            
        }
        public Friend(int newUserId)
        {
            FfriendId = 1;
            SfriendId = newUserId;
            WhoThink = null!;
        }
        public Friend(int fId, int sId)
        {
            FfriendId = fId;
            SfriendId = sId;
            WhoThink = sId;
        }
    }
}
