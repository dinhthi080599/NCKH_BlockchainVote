using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using System.Text.RegularExpressions;

namespace CSharpchainWebAPI.Models
{
    public class RSAEnc
    {
        private static RSACryptoServiceProvider csp;
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;
        public RSAEnc()
        {
            csp = new RSACryptoServiceProvider();
            _privateKey = csp.ExportParameters(true);
            _publicKey = csp.ExportParameters(false);
        }

        public string PublicKeyString()
        {
            return csp.ToXmlString(false);
        }

        public string PrivateKeyToString()
        {
            return csp.ToXmlString(true);
        }

        public string Encrypt(string plain_text)
        {
            csp.ImportParameters(_publicKey);
            var data = Encoding.Unicode.GetBytes(plain_text);
            var cypher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public string Decrypt(string cypher_text)
        {
            var dataBytes = Convert.FromBase64String(cypher_text);
            csp.ImportParameters(_privateKey);
            var plaintext = csp.Decrypt(dataBytes, false);
            return Encoding.Unicode.GetString(plaintext);
        }

        public string decrypt_with_pem(string cypher_text, string private_key)
        {
            var plaintext = "";
            try
            {
                var bytesToDecrypt = Convert.FromBase64String(cypher_text);
                AsymmetricCipherKeyPair keyPair;
                var pemReader = new PemReader(new StringReader(private_key));
                keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
                var decryptEngine = new Pkcs1Encoding(new RsaEngine());
                decryptEngine.Init(false, keyPair.Private);
                var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
                plaintext = decrypted.Replace("\0", "");
                return plaintext;
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        public void ExportPrivateKey()
        {
            using (StreamWriter outputStream = new StreamWriter("testRSA.pem"))
            {
                if (csp.PublicOnly) throw new ArgumentException("CSP does not contain a private key", "csp");
                var parameters = csp.ExportParameters(true);
                using (var stream = new MemoryStream())
                {
                    var writer = new BinaryWriter(stream);
                    writer.Write((byte)0x30); // SEQUENCE
                    using (var innerStream = new MemoryStream())
                    {
                        var innerWriter = new BinaryWriter(innerStream);
                        EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                        EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
                        EncodeIntegerBigEndian(innerWriter, parameters.Exponent);
                        EncodeIntegerBigEndian(innerWriter, parameters.D);
                        EncodeIntegerBigEndian(innerWriter, parameters.P);
                        EncodeIntegerBigEndian(innerWriter, parameters.Q);
                        EncodeIntegerBigEndian(innerWriter, parameters.DP);
                        EncodeIntegerBigEndian(innerWriter, parameters.DQ);
                        EncodeIntegerBigEndian(innerWriter, parameters.InverseQ);
                        var length = (int)innerStream.Length;
                        EncodeLength(writer, length);
                        writer.Write(innerStream.GetBuffer(), 0, length);
                    }

                    var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                    outputStream.WriteLine("-----BEGIN RSA PRIVATE KEY-----");
                    // Output as Base64 with lines chopped at 64 characters
                    for (var i = 0; i < base64.Length; i += 64)
                    {
                        outputStream.WriteLine(base64, i, Math.Min(64, base64.Length - i));
                        Console.Write(base64, i, Math.Min(64, base64.Length - i));
                    }
                    outputStream.WriteLine("-----END RSA PRIVATE KEY-----");
                }
            }
        }

        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }
        public string ExportPublicKey()
        {
            string pem = "";
            csp = new RSACryptoServiceProvider();
            _privateKey = csp.ExportParameters(true);
            _publicKey = csp.ExportParameters(false);
            RSAParameters parameters = _publicKey;
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }
                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                pem += "-----BEGIN PUBLIC KEY-----\r\n";
                for (var i = 0; i < base64.Length; i++)
                {
                    pem += base64[i];
                }
                pem += "\r\n-----END PUBLIC KEY-----";
            }
            return pem;
        }

        public string PrivateKey_PemConvertToXml(string PrivateKey_Pem)
        {
            RSAParameters p;
            var sr1 = "-----BEGIN PUBLIC KEY-----\r\n"
            + "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvwtEbLYHPW6jumNld3G5"
            + "rLDhXnbHlrCEsrc931nsNguv5XSaTm98jd2uRZFtLX / 0o5NKHiTteTTjkJIcYeHo"
            + "6w + DlMoFjfY8TsiEaHAUi1LpAy / CGVHsVhkv2IEjVFloqMSE4MvCNEwZWeKtn4lt"
            + "Tl8anQcTMjIskaSRbortqZ / riYaAW3VV0w91YC82YNdMVQwMGhi0n758liIujXHZ"
            + "bqbf6FqwuifI / Sq1shRT1XDxbjlaIpcqDb1mHT + UnNUJ3zE89CJk6cAnAqS3OPKC"
            + "pL / O8vFWOWZPtEGcwYFLg2zVSBX + qwZXciF5ZN16k / NRyamTgn / BbiJ + ja / EaoKK"
            + "ZQIDAQAB"
            + "\r\n-----END PUBLIC KEY-----";
            var sr2 = "-----BEGIN PRIVATE KEY-----\r\n"
            + "MIICXAIBAAKBgQCpHCHYgawzNlxVebSKXL7vfc / ihP + dQgMxlaPEi7 / vpQtV2szH"
            + "jIP34MnUKelXFuIETJjOgjWAjTTJoj38MQUWc3u7SRXaGVggqQEKH + cRi5 + UcEOb"
            + "Ifpi + cIyAm9MJqKabfJK2e5X / OS7FgAwPjgtDbZOZxamOrWWL8KGB + lH + QIDAQAB"
            + "AoGBAIXtL6jFWVjdjlZrIl4JgXUtkDt21PD33IuiVKZNft4NOWLu + wp17 / WZYn3S"
            + "C2fbSXfaKZIycKi0K8Ab6zcUo0 + QZKMoaG5GivnqqTPVAuZchkuMUSVgjGvKAC / D"
            + "12 / b + w + Shs9pvqED1CxfvtePXNwL6ZNuaREFC5hF / YpMVyg5AkEA3BUCZYJ + Ec96"
            + "2cwsdY6HocW8Kn + RIqMjkNtyLA19cQV5mpIP7kAiW6drBDlraVANi + 5AgK2zQ + ZT"
            + "hYzs / JfRKwJBAMS1g5 / B7XXnfC6VTRs8AMveZudi5wS / aGpaApybsfx1NTLLsm3l"
            + "GmGTkbCr + EPzvJ5zRSIAHAA6N6NdORwzEWsCQHTli + JTD5dyNvScaDkAvbYFi06f"
            + "d32IXYnBpcEUYT65A8BAOMn5ssYwBL23qf / ED431vLkcig1Ut6RGGFKKaQUCQEfa"
            + "UdkSWm39 / 5N4f / DZyySs + YO90csfK8HlXRzdlnc0TRlf5K5VyHwqDkatmoMfzh9G"
            + "1dLknVXL7jTjQZA2az8CQG0jRSQ599zllylMPPVibW98701Mdhb1u20p1fAOkIrz"
            + "+ BNEdOPqPVIyqIP830nnFsJJgTG2eKB59ym + ypffRmA ="
            + "\r\n-----END PRIVATE KEY-----";
            //PrivateKey_Pem.Replace("\n", "");
            PrivateKey_Pem = Regex.Replace(PrivateKey_Pem, "\n", "");
            PrivateKey_Pem = Regex.Replace(PrivateKey_Pem, "-----BEGIN RSA PRIVATE KEY-----", "-----BEGIN RSA PRIVATE KEY-----\r\n");
            PrivateKey_Pem = Regex.Replace(PrivateKey_Pem, "-----END RSA PRIVATE KEY-----", "\r\n-----END RSA PRIVATE KEY-----");
            var pemReader = new PemReader(new StringReader(sr2));
            var obj = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            //var obj = pemReader.ReadObject();
            if (obj is AsymmetricCipherKeyPair)
            {
                var key = (obj as AsymmetricCipherKeyPair).Private as RsaPrivateCrtKeyParameters;
                p = new RSAParameters
                {
                    Modulus = key.Modulus.ToByteArrayUnsigned(),
                    Exponent = key.PublicExponent.ToByteArrayUnsigned(),
                    D = key.Exponent.ToByteArrayUnsigned(),
                    P = key.P.ToByteArrayUnsigned(),
                    Q = key.Q.ToByteArrayUnsigned(),
                    DP = key.DP.ToByteArrayUnsigned(),
                    DQ = key.DQ.ToByteArrayUnsigned(),
                    InverseQ = key.QInv.ToByteArrayUnsigned(),
                };
            }
            else
            {
                return ("not support this pem");
                throw new NotSupportedException("not support this pem");
            }

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(p);
            return rsa.ToXmlString(true);
        }


        public string PublicKey_PemConvertToXml(string PublicKey_Pem)
        {
            RSAParameters p;
            var pemReader = new PemReader(new StringReader(PublicKey_Pem));
            var obj = pemReader.ReadObject();
            if (obj is RsaKeyParameters)
            {
                var key = obj as RsaKeyParameters;
                p = new RSAParameters
                {
                    Modulus = key.Modulus.ToByteArrayUnsigned(),
                    Exponent = key.Exponent.ToByteArrayUnsigned()
                };
            }
            else
            {
                return ("not support this pem");
                throw new NotSupportedException("not support this pem");
            }

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(p);
            return rsa.ToXmlString(false);
        }
        public string XmlToPem(string xml)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(xml);

                AsymmetricCipherKeyPair keyPair = Org.BouncyCastle.Security.DotNetUtilities.GetRsaKeyPair(rsa); // try get private and public key pair
                if (keyPair != null) // if XML RSA key contains private key
                {
                    PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
                    return FormatPem(Convert.ToBase64String(privateKeyInfo.GetEncoded()), "PRIVATE KEY");
                }

                RsaKeyParameters publicKey = Org.BouncyCastle.Security.DotNetUtilities.GetRsaPublicKey(rsa); // try get public key
                if (publicKey != null) // if XML RSA key contains public key
                {
                    SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
                    return FormatPem(Convert.ToBase64String(publicKeyInfo.GetEncoded()), "PUBLIC KEY");
                }
            }

            throw new InvalidKeyException("Invalid RSA Xml Key");
        }
        public string XmlConvertToPem(string xml)
        {
            var rsa2 = new RSACryptoServiceProvider();
            rsa2.FromXmlString(xml);
            var p = rsa2.ExportParameters(!rsa2.PublicOnly);
            AsymmetricKeyParameter key = null;
            //Private Key
            key = new RsaPrivateCrtKeyParameters(
                new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent), new BigInteger(1, p.D),
                new BigInteger(1, p.P), new BigInteger(1, p.Q), new BigInteger(1, p.DP), new BigInteger(1, p.DQ),
                new BigInteger(1, p.InverseQ));


            TextWriter textWriter = new StringWriter();
            var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(textWriter);
            pemWriter.WriteObject(key);
            pemWriter.Writer.Flush();
            return textWriter.ToString();
        }

        private string FormatPem(string pem, string keyType)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("-----BEGIN {0}-----\n", keyType);

            int line = 1, width = 64;

            while ((line - 1) * width < pem.Length)
            {
                int startIndex = (line - 1) * width;
                int len = line * width > pem.Length
                              ? pem.Length - startIndex
                              : width;
                sb.AppendFormat("{0}\n", pem.Substring(startIndex, len));
                line++;
            }

            sb.AppendFormat("-----END {0}-----\n", keyType);
            return sb.ToString();
        }
        long LongRandom(long min, long max, Random rand)
        {
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;
        }
    }
}