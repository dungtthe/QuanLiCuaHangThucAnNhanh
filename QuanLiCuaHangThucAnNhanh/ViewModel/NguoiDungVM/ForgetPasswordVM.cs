using QuanLiCuaHangThucAnNhanh.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Utils;
using QuanLiCuaHangThucAnNhanh.Model.Mapper;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM
{
    public class ForgetPasswordVM: BaseViewModel
    {
        public ICommand GuiMK { get; set; }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public ForgetPasswordVM()
        {
            GuiMK = new ViewModelCommand(accept);
        }

        // hàm gửi
        private async void accept(object obj)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBoxCustom.Show(MessageBoxCustom.Warning, "Không đủ thông tin");
                return;
            }
            var user = DataProvider.Ins.DB.NguoiDungs.FirstOrDefault(u => u.Email == Email);
            if (user == null) 
            {
                MessageBoxCustom.Show(MessageBoxCustom.Warning, "Email không đúng");
                return;
            }
            else
            {
                bool mailSent = await Task.Run(() => sendMail(Email));

                if (mailSent)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Đã gửi mật khẩu qua Email");
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Warning, "Có lỗi trong quá trình gửi tới Email liên kết!");
               
                }
                await Task.Delay(2000);
            }

            
        }

        //Tạo MK mới
        private static string RandomPasswordNew()
        {
            Random random = new Random();
            int passwordLength = random.Next(5, 11);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var password = new char[passwordLength];

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }
            return new string(password);
        }

        //Gửi mật khẩu qua Email
        private static bool sendMail( string mailReceive)
        {
            try
            {
                string fromMail = "privateclinicse104@gmail.com";
                string fromPassWord = "ibap lpjv sqrf vrsq";

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromMail);
                mailMessage.Subject = "Khôi phục mật khẩu";
                mailMessage.To.Add(new MailAddress(mailReceive));


                string noidung = "Mật khẩu mới của bạn là: ";
                string passwordNew = RandomPasswordNew();
                mailMessage.Body = "<html><body>" + noidung + "<b>" + passwordNew + "</b>" + "</body></html>";
                mailMessage.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassWord),
                    EnableSsl = true
                };
                smtpClient.Send(mailMessage);


                //sửa lại password trong bảng account theo username
                ObservableCollection<NguoiDung> list = new ObservableCollection<NguoiDung>(DataProvider.Ins.DB.NguoiDungs);
                foreach (NguoiDung item in list)
                {
                    if (mailReceive == item.Email)
                    {
                        item.MatKhau = MotSoPhuongThucBoTro.MD5Hash(passwordNew);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                }
                return true;
            }
            catch { return false; }
        }
    }
}
