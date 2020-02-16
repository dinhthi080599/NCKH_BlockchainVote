using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;

namespace CSharpchainWebAPI.Controllers
{
    public class VotingController : BaseController
    {
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
            ViewBag.dbc = dbc.get_dotbaucu_by_ID(id);
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
    }
}