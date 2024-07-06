using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using static System.Windows.Forms;

namespace QuanLiCuaHangThucAnNhanh.Utils
{
    public class MotSoPhuongThucBoTro
    {


        //bổ trợ phần mật khẩu cho người dùng
        public static string MD5Hash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
              
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }



        //chuyển Bitmap sang mảng Byte
        public static byte[] BitmapImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] data;
            PngBitmapEncoder encoder = new PngBitmapEncoder(); // Sử dụng PNG encoder
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                data = stream.ToArray();
            }

            return data;
        }


        /// Phương thức chuyển đổi mảng byte thành BitmapImage
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); 
                return bitmapImage;
            }
        }

        // Phương thức chuyển đổi đường dẫn ảnh thành mảng byte
        public static byte[] ImagePathToByteArray(string imagePath)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            return BitmapImageToByteArray(bitmapImage);
        }

        //phương thức tạo tên file ngẫu nhiên
        public static string RandomFileName(string type)
        {
            Random random = new Random();
            int passwordLength = random.Next(10, 20);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var password = new char[passwordLength];

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            string result = type + "_" + new string(password);
            return result;
        }


        //random password
        public static string RandomPassword()
        {
            Random random = new Random();
            int passwordLength = random.Next(10, 20);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var password = new char[passwordLength];

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }
            return new string(password);
        }



        public static string SelectFolder()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select a folder",
                Filter = "Folders|*.none",
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                return Path.GetDirectoryName(dialog.FileName);
            }
            return null;
        }
    }
}
