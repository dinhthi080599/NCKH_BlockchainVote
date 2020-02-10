using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using CSharpChainModel;
using CSharpChainServer;
using CSharpchainWebAPI.Models;

namespace CSharpchainWebAPI.Controllers
{
    public class WelComeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Trang chủ";
            ViewBag.hoTen = Session["sHovaten"]; // lấy tên trong session lúc đăng nhập
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
            ViewBag.block = WebApiApplication.CommandBlock(0);
            ViewBag.bll = WebApiApplication.CommandBlockchainLength();
            ViewBag.Title = "Hệ thống bỏ phiếu, bình chọn";
            return View();
        }
        public ActionResult Main()
        {
            return View();
        }
        int test = 1;

        public ActionResult all_block()
        {   if (this.   test == 1)
            {
                WebApiApplication.CommandBlockchainMine("admin");
                WebApiApplication.CommandBlockchainMine("admin");
                WebApiApplication.CommandBlockchainMine("admin");
                this.test = 4;
            }
            Block[] block = new Block[WebApiApplication.CommandBlockchainLength()];
            for (var i=0; i< WebApiApplication.CommandBlockchainLength(); i++)
            {
                block[i] = WebApiApplication.CommandBlock(i);
            }
            // ViewData["DotBauCu"] = dotBauCu;
            ViewBag.blockChain = block;
            ViewBag.bll = WebApiApplication.CommandBlockchainLength();
            ViewBag.Title = "Hệ thống bỏ phiếu, bình chọn";
            return View();
        }
    }
}
