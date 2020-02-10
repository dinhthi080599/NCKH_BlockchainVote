using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CSharpchainWebAPI.Controllers
{
    public class TestSendMailController : BaseController
    {
        // GET: TestSendMail
        public ActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult sendMail()
        //{
        //    return View();
        //}
        [HttpPost]
        public ViewResult Index(Models.TestSendMailController _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress("NCKHBlockchainVote@gmail.com");
                mail.Subject = "Test gửi mail";
                //string Body = _objModelMail.Body;
                string Body = "Đây là mail được gửi để kiểm tra tính năng gửi mail";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("NCKHBlockchainVote@gmail.com", "Admin10110!"); // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }
    }
}