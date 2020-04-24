using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;
namespace CSharpchainWebAPI.Controllers
{
    public class CreateSignatureController : BaseController
    {
        // GET: CreateSignature
        public ActionResult Index(int id)
        {
            checkCreatedSignature(id);
            ViewBag.title = "Tạo khóa riêng tư";
            ViewBag.id = id;
            return View();
        }
        public void checkCreatedSignature(int id)
        {
            int ma_cutri = int.Parse(Session["ma_taikhoan"].ToString());
            Signature sn = new Signature();
            if (sn.checkCreated(id, ma_cutri))
            {
                Response.Redirect("~/Voting/" + id);
            }
        }
        public List<string> createSignature(int id)
        {
            // int electorID = int.Parse(Request["id"]);
            int ma_cutri = int.Parse(Session["ma_taikhoan"].ToString());
            RSAEnc rsa = new RSAEnc();
            List<string> signature = new List<string>();
            signature.Add(rsa.XmlConvertToPem(rsa.PrivateKeyToString()));
            signature.Add(RSAConvert.XmlToPem(rsa.PublicKeyString()));
            string _private_key = LongRandom(1000000000000000, 999999999999999999, new Random()).ToString() + LongRandom(1000000000000000, 999999999999999999, new Random()).ToString();
            string cypher_private_key = rsa.Encrypt(_private_key);
            Signature sn = new Signature();
            var status = sn.InsertSignature(id, ma_cutri, signature, cypher_private_key);
            if (status == "false")
            {
                signature = createSignature(id);
            }
            else if (status == "exists")
            {
                return new List<string>();
            }
                
            return signature;
        }

        public JsonResult get_signature(int id)
        {
            return Json(createSignature(id), JsonRequestBehavior.AllowGet);
        }
        long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            return (Math.Abs(longRand % (max - min)) + min);
        }
        [HttpPost]
        public FileResult Download()
        {
            String private_key = Request["private_key"];
            byte[] fileBytes = Encoding.ASCII.GetBytes(private_key);
            string fileName = "private_key.txt";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

    }
}