using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSharpchainWebAPI.Models;
namespace CSharpchainWebAPI.Controllers
{
    public class CreateSignatureController : Controller
    {
        // GET: CreateSignature
        public ActionResult Index()
        {
            RSAEnc rsa = new RSAEnc();
            ViewBag._privateKeyString = rsa.PrivateKeyToString();
            ViewBag._publicKeyString = RSAConvert.PemToXml(rsa.XmlConvertToPem(rsa.PrivateKeyToString()));
            return View();
        }
    }
}