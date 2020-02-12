using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;
namespace CSharpchainWebAPI.Controllers
{
    public class ElectionController : BaseController
    {
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
            return View();
        }
        [HttpPost]
        public string createElector()
        {
            return "success";
        }
    }
}