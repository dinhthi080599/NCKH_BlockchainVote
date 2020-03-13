using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;
using CSharpChainModel;
using CSharpChainServer;

namespace CSharpchainWebAPI.Controllers
{
    public class VotingController : BaseController
    {
        List<Elector> el = null;
        public string get_action(int ID)
        {
            using(admin_voteEntities db = new admin_voteEntities())
                {
                var obj = db.tbl_dotbaucu
                    .Where(
                        a =>  a.ma_dot == ID
                    ).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.dThoigianbd > DateTime.Now)
                    {
                        return "Waiting";
                    }
                    else if (obj.dThoigiankt < DateTime.Now)
                    {
                        return "Finished";
                    }
                }
                else
                {
                    return "Error";
                }
            }
            return "Index";
        }
        // GET: Voting
        public ActionResult Index(int id)
        {
            string action = get_action(id);
            DotBauCu dbc = new DotBauCu();
            DotBauCu temp = new DotBauCu();
            temp = dbc.get_dotbaucu_by_ID(id);
            ViewBag.dbc = temp;
            Elector e = new Elector();
            el = e.getElectorbyId(id);
            ViewBag.ElectorList = el;
            switch (action)
            {
                case "Waiting":
                    {
                        ViewBag.title = "Bầu cử";
                        break;
                    }
                case "Finished":
                    {
                        ViewBag.title = "Bầu cử kết thúc";
                        Dictionary<string, int> number_of_vote = WebApiApplication.number_of_vote(id);
                        ViewBag.number_of_vote = number_of_vote;
                        break;
                    }
                case "Index":
                    {
                        break;
                    }
                case "Error":
                    {
                        RedirectToAction("Error");
                        break;
                    }
            }
            return View("~/Views/Voting/" + action + ".cshtml");
        }
        [HttpPost]
        public ActionResult get_elector(int id)
        {
            Elector e = new Elector();
            e = e.getElectorbyIdE(id);
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult vote(int id, int[] arrayID)
        {
            BlockchainServices blockchainServices = new BlockchainServices();
            String ma_tk = System.Web.HttpContext.Current.Session["ma_taikhoan"].ToString();
            List <Vote> list_vote = new List<Vote>();
            foreach(int i in arrayID)
            {
                Vote v = new Vote();
                v.voterID = ma_tk;
                v.voteParty = i;
                v.electorID = id;
                list_vote.Add(v);
            }
            Block block = new Block(DateTime.UtcNow, list_vote, blockchainServices.LatestBlock().Hash);
            BlockServices bs = new BlockServices(block);
            bs.MineBlock(4);
            WebApiApplication.CommandBlockchainMine(System.Web.HttpContext.Current.Session["sHovaten"].ToString(), block);
            ReadWriteData wd = new ReadWriteData();
            wd.write(block);
            return Json("taoBlockThanhCong", JsonRequestBehavior.AllowGet);
        }
    }
}