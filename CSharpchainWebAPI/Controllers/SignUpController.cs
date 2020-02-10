using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using CSharpchainWebAPI.Models;
//using System.Web.Http;

namespace CSharpchainWebAPI.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string register()
        {

            var sTendangnhap = Request["account"];
            var sMatkhau = Request["password"];
            var sHoTen = Request["user_name"];
            var reMK = Request["re_password"];
            var email = Request["email"];
            if(sTendangnhap != "")
            {
                using (admin_voteEntities db = new admin_voteEntities())
                {
                    var obj = db.tbl_taikhoan
                        .Where(
                            a => a.sTendangnhap.Equals(sTendangnhap)
                        ).FirstOrDefault();
                    if (obj != null)
                    {
                        return "taikhoan_tontai";
                    }
                }
            }
            if (email != "")
            {
                using (admin_voteEntities db = new admin_voteEntities())
                {
                    var obj = db.tbl_taikhoan
                        .Where(
                            a => a.sEmail.Equals(email)
                        ).FirstOrDefault();
                    if (obj != null)
                    {
                        return "email_tontai";
                    }
                }
            }
            if(sMatkhau != reMK)
            {
                return "matkhau_khongkhop";
            }
            if (sTendangnhap != "" && sMatkhau != "" && sHoTen != "" && sMatkhau == reMK)
            {
                var sha_matkhau = Sha256.sha256_hash(sMatkhau);
                using (admin_voteEntities db = new admin_voteEntities())
                {
                    var customers = db.Set<tbl_taikhoan>();
                    customers.Add(new tbl_taikhoan { sTendangnhap = sTendangnhap, sMatkhau = sha_matkhau, sHovaten = sHoTen, sEmail = email });
                    db.SaveChanges();
                }
                return "thanhcong";
            }
            else
            {
                return "thatbai";
            }
        }

    }
}