using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;


namespace BLL
{
    internal class NhanVienBLL
    {

        public bool GuiMail(string senderEmail, string senderPassword, string recipientEmail, string subject, string body)
        {
            
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    // Password là mật khẩu ứng dụng (không phải mật khẩu chính của google)
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                };

                MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail)
                {
                    Subject = subject,
                    Body = body
                };

                smtpClient.Send(mailMessage);
                Console.WriteLine("Email đã gửi thành công!");
                return true;
        }
    }
}
