using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCat.MyObjects
{
    public class UserImage
    {
        public int Id { get; set; }
        public byte[] ImageInfo { get; set; } = null!;
        public int UserId { get; set; }

        public UserImage()
        {
            
        }
        public UserImage(byte[] imageByte, int userId)
        {
            this.ImageInfo = imageByte;
            this.UserId = userId;
        }
    }
}
