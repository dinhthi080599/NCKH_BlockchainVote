using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using CSharpchainWebAPI.Models;
using System.Net.Mail;

namespace CSharpchainWebAPI.Controllers
{
    public class BaseController : Controller
    {
        public string base_url;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Session["ma_taikhoan"];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    System.Web.Routing.RouteValueDictionary(new { controller = "", action = "index" })
                    );
            }
            base.OnActionExecuting(filterContext);
        }
        public int RandomNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        public string get_base_url()
        {
            return "http://localhost:808" + Session["ma_taikhoan"].ToString();
        }
        public void SendMail(string Email, string NoiDung)
        {
            Models.TestSendMailController _objModelMail = new Models.TestSendMailController();
            _objModelMail.To = Email;
            MailMessage mail = new MailMessage();
            mail.To.Add(_objModelMail.To);
            mail.From = new MailAddress("NCKHBlockchainVote@gmail.com");
            mail.Subject = "Xác thực tài khoản NCKH BlockChain Vote";
            string Body = NoiDung;
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
        }
    }
}