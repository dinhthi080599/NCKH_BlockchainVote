using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;
using CSharpChainModel;
using CSharpChainServer;
using System.Net.Http;
using System.Threading.Tasks;

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
                        Dictionary<string, int> number_of_vote = new Dictionary<string, int>();
                        ViewBag.number_of_vote = number_of_vote;
                        break;
                    }
                case "Index":
                    {
                        ViewBag.voted = checkVoted(Session["ma_taikhoan"].ToString(), id);
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
            ReadWriteData wd = new ReadWriteData();
            wd.write(block);
            return Json("taoBlockThanhCong", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<string> add_vote(int id, int[] arrayID)
        {
            String ma_tk = System.Web.HttpContext.Current.Session["ma_taikhoan"].ToString();
            List<Vote> list_vote = new List<Vote>();
            foreach (int i in arrayID)
            {
                Vote v = new Vote();
                v.voterID = ma_tk;
                v.voteParty = i;
                v.electorID = id;
                list_vote.Add(v);
            }
            _ = await add_vote_(list_vote);
            return "taoBlockThanhCong";
        }
        static async Task<String> add_vote_(List<Vote> list_vote)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsJsonAsync<List<Vote>>("http://localhost:8080/api/Vote/VoteAdd", list_vote);
                    if(response.IsSuccessStatusCode)
                    {
                        var _response = await client.PostAsJsonAsync("http://localhost:8080/api/Blockchain/MineBlock", "");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  " + ex.Message);
                    Console.ResetColor();
                }
            }
            return "string";
        }

        public Boolean checkVoted(string voterID, int electorID)
        {
            String status = "";
            using (var client = new HttpClient())
            {
                try
                {
                    var response = client.PostAsJsonAsync<string>("http://localhost:8080/api/blockchain/checkVoted?voterID=" + voterID + "&&electorID=" + electorID.ToString(), "").Result;
                    //status = );
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  " + ex.Message);
                    Console.ResetColor();
                }
            }
            if(status == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}