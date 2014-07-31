// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JSONEncryptTest.cs" company="">
//   
// </copyright>
// <summary>
//   The json encrypt test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Ripple.Core.Tests.Unit.Crypto.SjclJson
{
    using System;
    using System.Security.Policy;
    using System.Text;
    using System.Web;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using Org.BouncyCastle.Crypto;

    using Ripple.Core.Crypto.Sjcljson;

    /// <summary>
    /// The json encrypt test.
    /// </summary>
    [TestFixture]
    public class JSONEncryptTest
    {
        /// <summary>
        /// The test decryption.
        /// </summary>
        [Test]
        public void testDecryption()
        {
            string fixture = "{\"key\" : \"user|pass\",   \"raw\":{\"one\":\"two\",\"three\":4},"
                             + "\"encrypted\":{\"iv\":\"OPiRt308ZbENmwzdDjffZQ==\"," + "\"v\":1," + "\"iter\":1000,"
                             + "\"ks\":256," + "\"ts\":64," + "\"mode\":\"ccm\"," + "\"adata\":\"\","
                             + "\"cipher\":\"aes\"," + "\"salt\":\"NFPNycJ3ea0=\","
                             + "\"ct\":\"sZo58l4VRX2KR9xAsbP/dIVc9QJ+0VCmTZ3jIMbO1w==\"}}";

            JObject parsed = JObject.Parse(fixture), 
                    raw = parsed.GetValue("raw").ToObject<JObject>(), 
                    encrypted = parsed.GetValue("encrypted").ToObject<JObject>(), 
                    decrypted, 
                    reencrypted;

            int ks = 256, iter = 1000, ts = 64;

            JsonEncrypt jsonEncrypt = new JsonEncrypt(ks, iter, ts);

            string key = parsed["key"].ToString();
            decrypted = jsonEncrypt.Decrypt(key, encrypted);

            Assert.AreEqual(decrypted.GetValue("one").ToString(), raw.GetValue("one").ToString());
            Assert.AreEqual(decrypted.GetValue("three").ToObject<int>(), raw.GetValue("three").ToObject<int>());

            reencrypted = jsonEncrypt.Encrypt(key, raw, "sucks to be you ...x!!!");
            jsonEncrypt.Decrypt(key, reencrypted);

            if (reencrypted["adata"] != null)
            {
                reencrypted["adata"] = "0000" + reencrypted.GetValue("adata");
            }
            else
            {
                reencrypted.Add("adata", "0000" + reencrypted.GetValue("adata"));
            }

            bool thrown = false;
            try
            {
                jsonEncrypt.Decrypt(key, reencrypted);
            }
            catch (InvalidCipherTextException e)
            {
                thrown = true;
            }

            Assert.True(thrown);
        }

        /// <summary>
        /// The test decryption 128 macsize.
        /// </summary>
        [Test]
        public void testDecryption128Macsize()
        {
            string fixture = "{" + "\"key\" : \"user|pass\", " + "\"raw\":{\"one\":\"two\",\"three\":4},"
                             + "\"encrypted\":" + "{" + "\"iv\":\"lgd/ZDGHEZOnbIXpViykXg==\"," + "\"v\":1,"
                             + "\"iter\":1000," + "\"ks\":256," + "\"ts\":128," + "\"mode\":" + "\"ccm\","
                             + "\"adata\":" + "\"wtf%20bbq%3F\"," + "\"cipher\":\"aes\"," + "\"salt\":\"NFPNycJ3ea0=\","
                             + "\"ct\":\"GTvZENQJ97HTZp2UvW1C9Bxf7KBVlfKiOaR82njTMk45L/dP+tEG\"" + "}" + "}";

            JObject parsed = JObject.Parse(fixture), 
                    raw = parsed.GetValue("raw").ToObject<JObject>(), 
                    encrypted = parsed.GetValue("encrypted").ToObject<JObject>(), 
                    decrypted;

            int ks = 256, iter = 1000, ts = 128;

            string key = parsed.GetValue("key").ToString();
            var jsonEncrypt = new JsonEncrypt(ks, iter, ts);
            decrypted = jsonEncrypt.Decrypt(key, encrypted);

            Assert.AreEqual(decrypted.GetValue("one").ToString(), raw.GetValue("one").ToString());
            Assert.AreEqual(decrypted.GetValue("three").ToObject<int>(), raw.GetValue("three").ToObject<int>());

            string adata = HttpUtility.UrlDecode(encrypted.GetValue("adata").ToString(), Encoding.UTF8);
            Assert.AreEqual("wtf bbq?", adata);
        }
    }
}