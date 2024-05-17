using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCat.MyObjects
{
    public class MyImage
    {
        [Key]
        public int Id { get; set; }
        public byte[] Image { get; set; } = null!;
        public int UserInfoId { get; set; }

        public MyImage()
        {
            
        }
        public MyImage(byte[] image, int userId)
        {
            this.Image = image;
            this.UserInfoId = userId;
        }
    }
}
