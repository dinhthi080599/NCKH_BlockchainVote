using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;
using CSharpChainModel;
using CSharpChainServer;
using CSharpChainNetwork;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Newtonsoft.Json;

namespace CSharpchainWebAPI.Controllers
{
    public class VotingController : BaseController
    {
        List<Elector> el = null;
        Dictionary<string, int> number_of_Vote;

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
                        checkCreatedSignature(ID);
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
            checkCreatedSignature(ID);
            return "Index";
        }
        // GET: Voting
        public ActionResult Index(int id)
        {
            string action = get_action(id);
            DotBauCu dbc = new DotBauCu();
            DotBauCu temp = new DotBauCu();
            Signature signature = new Signature();
            Elector e = new Elector();
            temp = dbc.get_dotbaucu_by_ID(id);
            ViewBag.dbc = temp;
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
                        get_number_of_vote(id);
                        ViewBag.number_of_vote = this.number_of_Vote;
                        break;
                    }
                case "Index":
                    {
                        ViewBag.public_key = signature.Get_PublicKey(id, int.Parse(Session["ma_taikhoan"].ToString()));
                        ViewBag.voted = checkVoted(Session["ma_taikhoan"].ToString(), id);
                        ViewBag.title = "Bầu cử";
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
        public async Task<string> add_vote(int id, int[] arrayID, string _private_key)
        {
            if(_ktkhoa(id, _private_key) == "saikhoa_riengtu")
            {
                // kiểm tra khóa riêng tư có chính xác k
                return "saikhoa_riengtu";
            }
            String ma_tk = System.Web.HttpContext.Current.Session["ma_taikhoan"].ToString();
            if (checkVoted(ma_tk, id)) {
                // kiểm tra nếu bỏ phiếu rồi thì k cho phép bỏ phiếu nữa
                return "banDaBoPhieu";
            }
            List<Vote> list_vote = new List<Vote>();
            Signature signature = new Signature();
            Digital_Signature digital_Signature = new Digital_Signature();
            RSAEnc rSAEnc = new RSAEnc();
            // khóa riêng tư của chữ ký số
            string cypher_private_key = signature.Get_Cypertext(id, int.Parse(Session["ma_taikhoan"].ToString())); // khóa riêng tư bị mã hóa
            string private_key = rSAEnc.decrypt_with_pem(cypher_private_key, _private_key); // khóa riêng tư đã giải mã
            string public_key = digital_Signature.GetPublicKeyFromPrivateKeyEx(private_key); // khóa công khai của chữ ký số
            foreach (int i in arrayID)
            {
                Vote v = new Vote();
                v.voterID = ma_tk;
                v.voteParty = i;
                v.electorID = id;
                v.public_key = public_key;
                list_vote.Add(v);
                v.signature = digital_Signature.GetSignature(private_key, v.vote_tostring());
            }
            string tt = await add_vote_(list_vote);
            if(tt =="thanhcong")
            {
                return "taoBlockThanhCong";
            }
            else
            {
                return "taoBlockThatBai";
            }
        }
        //static 
        async Task<String> add_vote_(List<Vote> list_vote)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsJsonAsync<List<Vote>>(base.get_base_url() + "/api/Vote/VoteAdd", list_vote);
                    if(response.IsSuccessStatusCode)
                    {
                        var _response = await client.PostAsJsonAsync(base.get_base_url() + "/api/Blockchain/MineBlock", "");
                    }
                }
                catch (Exception ex)
                {
                    return "thatbai";
                }
            }
            return "thanhcong";
        }

        public Boolean checkVoted(string voterID, int electorID)
        {
            bool status = false;
            using (var client = new HttpClient())
            {
                try
                {
                    var response = client.PostAsJsonAsync<string>(base.get_base_url() + "/api/blockchain/checkVoted?voterID=" + voterID + "&electorID=" + electorID.ToString(), "").Result;
                    status = response.Content.ReadAsAsync<bool>().Result;
                }
                catch (Exception ex)
                {
                }
            }
            return status;
        }

        public void checkCreatedSignature(int id)
        {
            int ma_cutri = int.Parse(Session["ma_taikhoan"].ToString());
            Signature sn = new Signature();
            if (!sn.checkCreated(id, ma_cutri))
            {
                Response.Redirect("~/CreateSignature/"+id);
                return;
            }
        }

        [HttpPost]
        public string kiemtrakhoa(int id)
        {
            string private_key = Request["private_key"];
            Signature st = new Signature();
            string cypher_text = st.Get_Cypertext( id, int.Parse(Session["ma_taikhoan"].ToString()));
            RSAEnc rs = new RSAEnc();
            string check = rs.decrypt_with_pem(cypher_text, private_key);
            if(check!="false")
            {
                return "chinhxac";
            }
            else
            {
                return "saikhoa_riengtu";
            }
        }

        private string _ktkhoa(int id, string private_key)
        {
            Signature st = new Signature();
            string cypher_text = st.Get_Cypertext(id, int.Parse(Session["ma_taikhoan"].ToString()));
            RSAEnc rs = new RSAEnc();
            string check = rs.decrypt_with_pem(cypher_text, private_key);
            if (check != "false")
            {
                return "chinhxac";
            }
            else
            {
                return "saikhoa_riengtu";
            }
        }

        public async Task get_number_of_vote(int id)
        {
            number_of_Vote = new Dictionary<string, int>();
            string url = base.get_base_url() + "/api/blockchain/number_of_vote?electorID=" + id;
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                var status = response.Content.ReadAsAsync<Dictionary<string, int>>().Result;
                number_of_Vote = status;
            }
        }
    }
}