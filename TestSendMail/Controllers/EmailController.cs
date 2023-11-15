using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace TestSendMail.Controllers
{
    public class EmailController : Controller
    {
        public ActionResult SendEmail()
        {
            // Thông tin tài khoản email
            string senderEmail = "plantgreenroot@gmail.com";
            string senderPassword = "GreenRoot@123";

            // Thông tin người nhận
            string toEmail = "sangvu2015dp1@gmail.com";
            string subject = "Test Email";
            string body = "This is a test email.";

            // Tạo đối tượng MailMessage
            MailMessage mail = new MailMessage(senderEmail, toEmail, subject, body);

            // Cấu hình thông tin SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.example.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            // Gửi email
            try
            {
                smtpClient.Send(mail);
                ViewBag.Message = "Email sent successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Failed to send email: " + ex.Message;
            }

            return View();
        }
    }
}