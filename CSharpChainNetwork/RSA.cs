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
namespace CSharpChainNetwork
{
    public class RSAEnc
    {
        private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public RSAEnc()
        {
            _privateKey = csp.ExportParameters(true);
            _publicKey = csp.ExportParameters(false);
        }

        public string PublicKeyString()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);
            return sw.ToString();
        }

        public string PrivateKeyToString()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _privateKey);
            return sw.ToString();
        }

        public string Encrypt(string plain_text)
        {
            csp = new RSACryptoServiceProvider();
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
            using (StreamWriter outputStream = new StreamWriter("testRSA.pem"))
            {
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
                    outputStream.WriteLine("-----BEGIN PUBLIC KEY-----");
                    for (var i = 0; i < base64.Length; i += 64)
                    {
                        outputStream.WriteLine(base64, i, Math.Min(64, base64.Length - i));
                        Console.Write(base64, i, Math.Min(64, base64.Length - i));
                        pem += (base64, i, Math.Min(64, base64.Length - i)).ToString();
                    }
                    outputStream.WriteLine("-----END PUBLIC KEY-----");
                }
            }
            return pem;
        }

        public void PemConvertToXml(string pemPath, string xmlPath, bool generatePrivateKey = true)
        {
            RSAParameters p;
            using (var sr = File.OpenText(pemPath))
            {
                var sr1 = "-----BEGIN PUBLIC KEY-----\r\n"
                + "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvwtEbLYHPW6jumNld3G5"
                + "rLDhXnbHlrCEsrc931nsNguv5XSaTm98jd2uRZFtLX / 0o5NKHiTteTTjkJIcYeHo"
                + "6w + DlMoFjfY8TsiEaHAUi1LpAy / CGVHsVhkv2IEjVFloqMSE4MvCNEwZWeKtn4lt"
                + "Tl8anQcTMjIskaSRbortqZ / riYaAW3VV0w91YC82YNdMVQwMGhi0n758liIujXHZ"
                + "bqbf6FqwuifI / Sq1shRT1XDxbjlaIpcqDb1mHT + UnNUJ3zE89CJk6cAnAqS3OPKC"
                + "pL / O8vFWOWZPtEGcwYFLg2zVSBX + qwZXciF5ZN16k / NRyamTgn / BbiJ + ja / EaoKK"
                + "ZQIDAQAB"
                + "\r\n-----END PUBLIC KEY-----";
                var pemReader = new PemReader(new StringReader(sr1));
                var obj = pemReader.ReadObject();
                if (obj is RsaKeyParameters)
                {
                    var key = obj as RsaKeyParameters;
                    p = new RSAParameters
                    {
                        Modulus = key.Modulus.ToByteArrayUnsigned(),
                        Exponent = key.Exponent.ToByteArrayUnsigned()
                    };

                    //Public Key cant Convert To Private Key
                    generatePrivateKey = false;
                }
                else if (obj is AsymmetricCipherKeyPair)
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
                    throw new NotSupportedException("not support this pem");
                }
            }

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(p);
            File.WriteAllText(xmlPath, rsa.ToXmlString(generatePrivateKey)); //True: PrivateKey, False PublicKey
        }
        public static void XmlConvertToPem(string xmlPath, string pemPath, bool generatePrivateKey = true)
        {
            var sr1 = "-----BEGIN PUBLIC KEY-----\r\n"
                   + "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvwtEbLYHPW6jumNld3G5"
                   + "rLDhXnbHlrCEsrc931nsNguv5XSaTm98jd2uRZFtLX / 0o5NKHiTteTTjkJIcYeHo"
                   + "6w + DlMoFjfY8TsiEaHAUi1LpAy / CGVHsVhkv2IEjVFloqMSE4MvCNEwZWeKtn4lt"
                   + "Tl8anQcTMjIskaSRbortqZ / riYaAW3VV0w91YC82YNdMVQwMGhi0n758liIujXHZ"
                   + "bqbf6FqwuifI / Sq1shRT1XDxbjlaIpcqDb1mHT + UnNUJ3zE89CJk6cAnAqS3OPKC"
                   + "pL / O8vFWOWZPtEGcwYFLg2zVSBX + qwZXciF5ZN16k / NRyamTgn / BbiJ + ja / EaoKK"
                   + "ZQIDAQAB"
                   + "\r\n-----END PUBLIC KEY-----";
            //var pemReader = new PemReader();
            var rsa2 = new RSACryptoServiceProvider();
            rsa2.FromXmlString(sr1);
            var p = rsa2.ExportParameters(!rsa2.PublicOnly);

            //Public Key Convert to Private Key
            if (rsa2.PublicOnly)
            {
                generatePrivateKey = false;
            }

            AsymmetricKeyParameter key = null;
            if (generatePrivateKey)
            {
                //Private Key
                key = new RsaPrivateCrtKeyParameters(
                    new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent), new BigInteger(1, p.D),
                    new BigInteger(1, p.P), new BigInteger(1, p.Q), new BigInteger(1, p.DP), new BigInteger(1, p.DQ),
                    new BigInteger(1, p.InverseQ));
            }
            else
            {
                //Public key
                key = new RsaKeyParameters(false, new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent)); //Public Key
            }

            using (var sw = new StreamWriter(pemPath))
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(sw);
                pemWriter.WriteObject(key);
            }
        }

    }
}