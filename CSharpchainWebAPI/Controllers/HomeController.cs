using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EnableCorsAttribute = System.Web.Http.Cors.EnableCorsAttribute;
using CSharpchainWebAPI.Models;
using System.Net.Http;

namespace CSharpchainWebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
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
            var sMatkhau = Sha256.sha256_hash(Request["mat_khau"]);
            using (admin_voteEntities db = new admin_voteEntities())
            {
                var obj = db.tbl_taikhoan
                    .Where(
                        a => a.sTendangnhap.Equals(sTendangnhap) && a.sMatkhau.Equals(sMatkhau)
                    ).FirstOrDefault();
                if(obj != null)
                {
                    this.add_node(obj.ma_taikhoan.ToString());
                    Session["ma_taikhoan"] = obj.ma_taikhoan.ToString();
                    Session["ma_quyen"] = obj.ma_quyen.ToString();
                    Session["sHovaten"] = obj.sHovaten.ToString();
                    return Redirect("~/Welcome");
                }
            }
            return Redirect("~/");
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
            using (admin_voteEntities db = new admin_voteEntities())
            {
                var dotbaucu = db.Set<tbl_dotbaucu>();
                dotbaucu.Add(new tbl_dotbaucu { sTendot = ten_dotbophieu, sGhichu = mota_dotbophieu, dThoigianbd = dt1, dThoigiankt = dt2 });
                db.SaveChanges();
            }
            return RedirectToAction("/Them_BauCu");
        }
        public string add_node(String new_node)
        {
            using (var client = new HttpClient())
            {
                string url = "http://localhost:8080/api/blockchain/AddNode?node="+new_node;
                string node = new_node;
                client.BaseAddress = new Uri(url);
                var response = client.PostAsJsonAsync("", node).Result;
                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }
                else
                    return "Error";
            }
        }

    }

}
