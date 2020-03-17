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
                    var response = await client.PostAsJsonAsync<string>("http://localhost:8080/api/Network/Unregister?RemoveNodeUrl="+ RemoveNodeUrl, "");
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
    }
}