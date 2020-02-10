using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;

namespace CSharpchainWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        [HttpPost]
        public ActionResult check_login()
        {
            var sTendangnhap = Request["tai_khoan"];
            var sMatkhau = Request["mat_khau"];
            using (admin_voteEntities db = new admin_voteEntities())
            {
                var obj = db.tbl_taikhoan
                    .Where(
                        a => a.sTendangnhap.Equals(sTendangnhap) && a.sMatkhau.Equals(sMatkhau)
                    ).FirstOrDefault();
                if(obj != null)
                {
                    Session["ma_taikhoan"] = obj.ma_taikhoan.ToString();
                    Session["ma_quyen"] = obj.ma_quyen.ToString();
                    Session["sHovaten"] = obj.sHovaten.ToString();
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult them_dotbaucu()
        {
            var ten_dotbophieu = Request["ten_dotbophieu"];
            var mota_dotbophieu = Request["mota_dotbophieu"];
            var thoigian_batdau = Request["thoigian_batdau"];
            var thoigian_ketthuc = Request["thoigian_ketthuc"];
            DateTime dt1 = DateTime.ParseExact(thoigian_batdau, "M/d/yyyy", CultureInfo.InvariantCulture);
            DateTime dt2 = DateTime.ParseExact(thoigian_ketthuc, "M/d/yyyy", CultureInfo.InvariantCulture);
            // insert
            using (admin_voteEntities db = new admin_voteEntities())
            {
                var dotbaucu = db.Set<tbl_dotbaucu>();
                dotbaucu.Add(new tbl_dotbaucu { sTendot = ten_dotbophieu, sGhichu = mota_dotbophieu, dThoigianbd = dt1, dThoigiankt = dt2 });
                db.SaveChanges();
            }
            return RedirectToAction("/Them_BauCu");
        }
    }

}
