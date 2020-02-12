using System;
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
        public string them_dotbaucu()
        {
            DotBauCu dbc = new DotBauCu();
            dbc.sTenDot = Request["ten_dot"];
            dbc.dThoiGianBD = Request["thoigian_bd"];
            dbc.dThoiGianKT = Request["thoigian_kt"];
            dbc.sNoiDung = Request["noidung"];
            dbc.iTrangThai = 1;
            dbc.sHinhThuc= Request["hinhthuc"];
            dbc.sSoPhieu = Request["sophieu"];
            dbc.iNguoiTao = Convert.ToInt32(Session["ma_taikhoan"]);
            return dbc.them_dotbaucu(dbc);
        }
    }
}