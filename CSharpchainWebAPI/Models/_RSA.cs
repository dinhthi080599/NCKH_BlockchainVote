using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace CSharpchainWebAPI.Models
{
    public class _RSA
    {
        RSACryptoServiceProvider rsa;
        public string _privateKeyXML;
        public string _publicKeyXML;

        public _RSA()
        {
            rsa = new RSACryptoServiceProvider();
            _privateKeyXML = rsa.ToXmlString(true);
            _publicKeyXML = rsa.ToXmlString(false);
        }

    }
}