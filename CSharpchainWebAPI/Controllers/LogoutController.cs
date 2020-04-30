using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CSharpChainModel;
using CSharpChainNetwork;

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
            WebApiApplication.node_list.Remove(RemoveNodeUrl);
            using (var client = new HttpClient())
            {
                try
                {
                    //BaseController bc = new BaseController();
                    //string url = bc.get_base_url() + "/api/Network/AddNode?node=" + RemoveNodeUrl;
                    //string url = "http://localhost:808" + RemoveNodeUrl + "/api/blockchain/AddNode?RemoveNode=" + RemoveNodeUrl;
                    //client.DeleteAsync(url).ConfigureAwait(false);
                    //NodeServices x = new NodeServices().RemoveNode(url);
                    //var response = await client.PostAsJsonAsync(url, "");
                    string url = "http://localhost:808" + RemoveNodeUrl + "/api/Network/Unregister?RemoveNode=" + RemoveNodeUrl;
                    client.BaseAddress = new Uri(url);
                    var response = await client.PostAsJsonAsync("", RemoveNodeUrl);
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