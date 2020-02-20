using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;
namespace CSharpchainWebAPI.Controllers
{
    public class ElectionController : BaseController
    {
        List<Elector> el = null;
        // GET: Election
        public ActionResult Index(int ID)
        {
            DotBauCu dbc = new DotBauCu();
            ElectionStatus electionStatus = new ElectionStatus();
            ViewBag.Title = "Đợt bầu cử:";
            Dictionary<string, ElectionStatus> dict = electionStatus.get_trangthai_dotbaucu();
            ViewBag.dotBauCu = dbc.get_dotbaucu_by_ID(ID);
            int i = (int)ViewBag.dotBauCu.iTrangThai;
            ViewBag.ES = dict[dict.Keys.ElementAt(i)];
            Elector e = new Elector();
            ViewBag.ElecterModal = new Elector();
            el = e.getElectorbyId(ID);
            ViewBag.ElectorList = el;
            ViewBag.ElectorCount = el.Count();
            return View();
        }

        [HttpPost]
        public JsonResult createElector()
        {
            var sHoten = Request["sHoten"];
            var sDiachi = Request["sDiachi"];
            var sEmail = Request["sEmail"];
            var dbcid = Request["dbcId"];
            var dNgaysinh = Request["dNgaysinh"];
            var bGioitinh = Request["bGioitinh"];
            var sGhichu = Request["sGhichu"];
            DateTime dt1 = DateTime.ParseExact(dNgaysinh, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            Elector e = new Elector();
            if (e.createNewElector(sHoten, bGioitinh, dNgaysinh, sEmail, sDiachi, dbcid, sGhichu))
            {
                el = e.getElectorbyId(int.Parse(dbcid));
                return Json(el);
            }
            else
                return null;
        }

        [HttpPost]
        public JsonResult getDetailElector()
        {
            Elector e = new Elector(), result = new Elector();
            var i = 0;
            var selectedId = long.Parse(Request["selectedId"]);
            var ID = int.Parse(Request["id"]);
            el = e.getElectorbyId(ID);
            for (i = 0; i < el.Count(); i++)
            {
                if (el[i].ma_ungcuvien == selectedId)
                {
                    result = el[i];
                }
            }
            return Json(result);
        }
    }
}