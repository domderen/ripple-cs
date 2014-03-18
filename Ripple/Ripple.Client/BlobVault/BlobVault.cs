using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using Ripple.Core.Crypto.Sjcljson;
using Ripple.Core.Encodings.Common;

namespace Ripple.Client.BlobVault
{
    public class BlobVault
    {
        private readonly string _baseUrl;
        private readonly JsonEncrypt _scjl = new JsonEncrypt();

        public BlobVault(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<JObject> GetBlob(string username, string password)
        {
            // Everywhere, this is expected to be lower cased.
            username = username.ToLower();

            string userPassUrl = UserPassHash(username, password);
            var client = new HttpClient { BaseAddress = new Uri(_baseUrl), Timeout = new TimeSpan(0, 0, 5) };
            var response = await client.GetAsync(userPassUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.NotFound || responseContent.Length == 0)
            {
                // We won't log the pass.
                throw new BlobNotFoundException("No blob found for user: " + username);
            }

            string utf8 = Encoding.UTF8.GetString(Base64.Decode(responseContent));
            string decryptionKey;

            try
            {
                decryptionKey = UserPassDerivedDecryptionKey(username, password);
                return _scjl.Decrypt(decryptionKey, utf8);
            }
            catch (InvalidCipherTextException)
            {
                decryptionKey = UserPassDerivedDecryptionKeyOld(username, password);
                return _scjl.Decrypt(decryptionKey, utf8);
            }
        }

        private string UserPassHash(string username, string password)
        {
            string toHash = username + password;

            try
            {
                byte[] toHashBytes = Encoding.UTF8.GetBytes(toHash);
                byte[] sha256 = new SHA256CryptoServiceProvider().ComputeHash(toHashBytes);
                return B16.ToString(sha256);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Hashing username with password failed.", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">Username already lower cased.</param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string UserPassDerivedDecryptionKey(string username, string password)
        {
            return username.Length + "|" + username + password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">Username already lower cased.</param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string UserPassDerivedDecryptionKeyOld(string username, string password)
        {
            return username + password;
        }
    }
}
