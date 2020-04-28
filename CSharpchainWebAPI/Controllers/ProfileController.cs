using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;
using System.Globalization;

namespace CSharpchainWebAPI.Controllers
{
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index()
        {
            DotBauCu dbc = new DotBauCu();
            int ma_tk = int.Parse(Session["ma_taikhoan"].ToString());
            Account account = new Account();
            account = account.GetAccount_byID(ma_tk);
            HashSet<long> list_voted = account.list_voted_byID(ma_tk);
            List<DotBauCu> list_dbc = new List<DotBauCu>();
            list_dbc = dbc.get_dotbaucu_by_array(list_voted.ToArray());
            ViewBag.soDot = list_dbc.Count();
            ViewBag.list_dbc = list_dbc;
            ViewBag.account = account;
            ViewBag.Title = "Hồ sơ cá nhân";
            return View();
        }

        // update info
        [HttpPost]
        public bool EditInfor()
        {
            DateTime dt1;
            var Name = Request["Name"];
            var Birthday = Request["Birthday"];
            var Address = Request["Address"];
            var Email = Request["Email"];
            var Gender = Request["Gender"];
            var Phone = Request["Phone"];
            var Id = long.Parse(Request["id"]);
            if (Birthday.Length != 0)
                dt1 = DateTime.ParseExact(Birthday, "dd/mm/yyyy", CultureInfo.InvariantCulture);
            Account account = new Account();
            if (account.EditAccountInfo(Id, Name, Gender, Birthday.ToString(), Phone, Email, Address)) return true;
            else return false;
        }
    }
}