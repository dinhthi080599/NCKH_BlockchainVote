using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CSharpChainModel;

namespace CSharpchainWebAPI.Controllers
{
    public class LogoutController : BaseController
    {
        // GET: Logout
        public ActionResult Index()
        {
            Unregister(Session["ma_taikhoan"].ToString());
            Session.Clear();
            return RedirectToAction("/");
        }

        static async Task<String> Unregister(String RemoveNodeUrl)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    BaseController bc = new BaseController();
                    string url = bc.get_base_url() + "/api/Network/Unregister?RemoveNode=" + RemoveNodeUrl;
                    client.BaseAddress = new Uri(url);
                    var response = await client.PostAsJsonAsync(url, "");
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Error";
                }
            }

        }
    }
}