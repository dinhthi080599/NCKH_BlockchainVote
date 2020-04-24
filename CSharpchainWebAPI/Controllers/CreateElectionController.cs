using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;

namespace CSharpchainWebAPI.Controllers
{
    public class CreateElectionController : BaseController
    {
        // GET: CreateElection
        public ActionResult Index()
        {
            ViewBag.Title = "Tạo đợt bỏ phiếu";
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public string them_dotbaucu()
        {
            DotBauCu dbc = new DotBauCu();
            dbc.sTenDot = Request["ten_dot"];
            dbc.dThoiGianBD = Request["thoigian_bd"];
            dbc.dThoiGianKT = Request["thoigian_kt"];
            dbc.sNoiDung = Request["noidung"].ToString();
            dbc.iTrangThai = 1; 
            dbc.sHinhThuc = Request["hinhthuc"];
            dbc.sSoPhieu = Request["sophieu"];
            dbc.iNguoiTao = Convert.ToInt32(Session["ma_taikhoan"]);
            List<UngCuVien> ucv = new List<UngCuVien>();
            var tt = dbc.them_dotbaucu(dbc);
            if (tt != "them_thatbai")
            {
                int ma_dbc = Int32.Parse(tt);
                return ma_dbc.ToString();
            }
            else
            {
                return "them_thatbai";
            }
        }

        [HttpPost]
        public string them_ungvien_dbc(List<UngCuVien> array_ungvien)
        {
            UngCuVien abc = new UngCuVien();
            return abc.themUngCuVien(array_ungvien);
        }
    }
}