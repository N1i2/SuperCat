using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCat.MyObjects
{
    class Friend
    {
        public int Id { get; set; }
        public int FirstID { get; set; }
        public int SecondID { get; set; }
        public int? Think { get; set; } = null;
        public Friend(int f_1, int f_2)
        {
            FirstID = f_1;
            Think = f_2;
        }
        public Friend()
        {
            
        }
    }
}
