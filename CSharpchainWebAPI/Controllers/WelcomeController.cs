using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSharpChainModel;
using CSharpchainWebAPI.Models;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Cors;
using EnableCorsAttribute = System.Web.Http.Cors.EnableCorsAttribute;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.IO;

namespace CSharpchainWebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class WelComeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Trang chủ";
            ViewBag.hoTen = Session["sHovaten"];
            return View();
        }
        public ActionResult DS_DotBoPhieu()
        {
            
            return View();
        }
        public ActionResult DS_Dathamgia()
        {
            DotBauCu dbc = new DotBauCu();

            ViewBag.dbc = dbc.get_dotbaucu();
            return View();
        }
        public ActionResult HuongDan_SuDung()
        {
            return View();
        }
        public ActionResult ChiTiet_DotBauCu()
        {
            return View();
        }
        public ActionResult Them_BauCu()
        {
            return View();
        }
        public ActionResult ThongTin_CaNhan()
        {
            return View();
        }
        public ActionResult Trangchu_Hello()
        {
            // ViewData["DotBauCu"] = dotBauCu;
            ViewBag.Title = "Hệ thống bỏ phiếu, bình chọn";
            return View();
        }
        public ActionResult Main()
        {
            return View();
        }
        public ActionResult all_block()
        {   
            // ViewData["DotBauCu"] = dotBauCu;
            ViewBag.Title = "Hệ thống bỏ phiếu, bình chọn";
            return View();
        }
    }
}
