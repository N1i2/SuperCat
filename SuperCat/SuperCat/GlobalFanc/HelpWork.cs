using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic.ApplicationServices;
using System.IO;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SuperCat.GlobalFanc
{
    static class HelpWork
    {
        public static string RecreateListByString(List<string> groupInclude, int without = -1)
        {
            string result = string.Empty;

            foreach (var ch in groupInclude)
            {
                if (int.TryParse(ch, out int i) && i != without)
                {
                    result += "." + i;
                }
            }

            return result;
        }
        public static string UnboxListByString(List<int> groupInclude, int without = -1)
        {
            string result = string.Empty;

            foreach (var ch in groupInclude)
            {
                if(ch != without)
                {
                    result += "." + ch;
                }
            }

            return result;
        }
        public static List<int> UnboxStringByList(string groupInclude, int without = -1)
        {
            List<int> result = new List<int>();

            foreach (var ch in groupInclude.Split("."))
            {
                if (int.TryParse(ch, out int i) && i != without)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        public static string UnboxStringByString(string userInclude, int without = -1)
        {
            string result = string.Empty;

            foreach (var chi in userInclude.Split("."))
            {
                if (int.TryParse(chi, out int i) && i != without)
                {
                    result += "." + i;
                }
            }

            return result;
        }

        public static byte[] GetBytesImageSource(ImageSource imageSource)
        {
            var bitmapSource = imageSource as BitmapSource;

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
        public static byte[] GetBytesBitmap(BitmapImage img)
        {
            byte[] imageBytes;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(img));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                imageBytes = ms.ToArray();
            }

            return imageBytes;
        }
        public static BitmapImage LoadImageFromByte(byte[] img)
        {
            using (MemoryStream mem = new MemoryStream(img))
            {
                BitmapImage bit = new BitmapImage();
                bit.BeginInit();
                bit.CacheOption = BitmapCacheOption.OnLoad;
                bit.StreamSource = mem;

                bit.EndInit();
                bit.Freeze();

                return bit;
            }
        }
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }
        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }
    }
}
