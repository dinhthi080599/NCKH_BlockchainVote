using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;


namespace CSharpchainWebAPI.Models
{
    public class Signature
    {
        public bool checkCreated(int electorID, int ma_cutri)
        {
            using(admin_voteEntities db = new admin_voteEntities())
            {
                if (db.tbl_signature.Any(o => o.ma_dotbaucu == electorID && o.ma_cutri == ma_cutri))
                {
                    return true;
                }
            }
            return false;
        }

        public string InsertSignature(int electorID, int ma_cutri, List<String> list_signature, string cypher_private_key)
        {
            string public_key = list_signature[1];
            using (admin_voteEntities db = new admin_voteEntities())
            {
                if (db.tbl_signature.Any(o => o.ma_cutri == ma_cutri && o.ma_dotbaucu == electorID))
                {
                    return "exists";
                }
                if (db.tbl_signature.Any(o => o.public_key.StartsWith(public_key) && o.public_key.EndsWith(public_key)))
                {
                    return "false";
                }
                var signature = db.Set<tbl_signature>();
                signature.Add(new tbl_signature {
                    ma_dotbaucu  = electorID, 
                    ma_cutri = ma_cutri, 
                    cypher_private_key1 = cypher_private_key, 
                    public_key = list_signature[1]
                });
                db.SaveChanges();
            }
            return "true";
        }

        long LongRandom(ulong min, ulong max, Random rand)
        {
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;
        }

        public string GetPublicKeyFromPrivateKeyEx(string privateKey)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var d = new Org.BouncyCastle.Math.BigInteger(privateKey);
            var q = domain.G.Multiply(d);
            var publicKey = new ECPublicKeyParameters(q, domain);
            return Base58Encoding.Encode(publicKey.Q.GetEncoded());
        }

        public string Get_Cypertext(int electorID, int ma_cutri)
        {
            string cypher_text = "";
            using (var context = new admin_voteEntities())
            {
                var posts = context.tbl_signature
                                   .Where(p => p.ma_cutri == ma_cutri && p.ma_dotbaucu == electorID)
                                   .Select(p => new { p.cypher_private_key1 }).FirstOrDefault();
                cypher_text = posts.cypher_private_key1;
            }
            return cypher_text;
        }

        public string Get_PublicKey(int electorID, int ma_cutri)
        {
            string public_key = "";
            using (var context = new admin_voteEntities())
            {
                var posts = context.tbl_signature
                                   .Where(p => p.ma_cutri == ma_cutri && p.ma_dotbaucu == electorID)
                                   .Select(p => new { p.public_key }).FirstOrDefault();
                if (!object.ReferenceEquals(posts, null))
                    public_key = posts.public_key;
            }
            return public_key;
        }

    }
}