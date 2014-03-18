using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;

namespace Ripple.Core.Crypto.Sjcljson
{
    public class JsonEncrypt
    {
        private readonly int _ks = 256;
        private readonly int _iter = 1000;
        private readonly int _ts = 64;

        private const string Mode = "ccm";

        /// <summary>
        /// Much credit for this class goes to Matthew Fettig
        /// https://github.com/AurionFinancial/AndroidWallet/blob/master/src/com/ripple/Blobvault.java
        /// This supports ccm mode encrypted data.
        /// </summary>
        /// <param name="ks"></param>
        /// <param name="iter"></param>
        /// <param name="ts"></param>
        public JsonEncrypt(int ks, int iter, int ts)
        {
            _ks = ks;
            _iter = iter;
            _ts = ts;
        }

        public JsonEncrypt()
        {
        }

        public JObject Encrypt(string key, JObject blob, string adata)
        {
            var result = new JObject();
            var random = new SecureRandom();

            var iv = new byte[32];
            var salt = new byte[8];

            random.NextBytes(salt);
            random.NextBytes(iv);

            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(blob.ToString());
                byte[] adataBytes = Encoding.UTF8.GetBytes(adata);
                byte[] nonce = ComputeNonce(iv, plainBytes);

                KeyParameter keyParam = CreateKey(key, salt, _iter, _ks);
                var ccm = new AeadParameters(keyParam, MacSize(_ts), nonce, adataBytes);

                var aes = new CcmBlockCipher(new AesFastEngine());
                aes.Init(true, ccm);

                var enc = new byte[aes.GetOutputSize(plainBytes.Length)];

                int res = aes.ProcessBytes(plainBytes, 0, plainBytes.Length, enc, 0);

                aes.DoFinal(enc, res);

                result.Add("ct", Base64.ToBase64String(enc));
                result.Add("iv", Base64.ToBase64String(iv));
                result.Add("salt", Base64.ToBase64String(salt));
                result.Add("adata", EncodeAdata(adata));
                result.Add("mode", Mode);
                result.Add("ks", _ks);
                result.Add("iter", _iter);
                result.Add("ts", _ts);

                return result;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Json encryption failed.", e);
            }
        }

        public JObject Decrypt(string key, string json)
        {
            try
            {
                return Decrypt(key, JObject.Parse(json));
            }
            catch (JsonException e)
            {
                throw new ApplicationException("Json decryption failed.", e);
            }
        }

        public JObject Decrypt(string key, JObject json)
        {
            try
            {
                byte[] iv = Base64.Decode(json.GetValue("iv").ToString());
                byte[] cipherText = Base64.Decode(json.GetValue("ct").ToString());
                byte[] adataBytes = DecodeAdataBytes(json.GetValue("adata").ToString());
                byte[] nonce = ComputeNonce(iv, cipherText);

                if (json.GetValue("mode").ToString() != "ccm")
                {
                    throw new ApplicationException("Can only decrypt ccm mode encrypted data.");
                }

                KeyParameter keyParam = CreateKey(
                    key,
                    Base64.Decode(json.GetValue("salt").ToString()),
                    json.GetValue("iter").ToObject<int>(),
                    json.GetValue("ks").ToObject<int>());

                var ccm = new AeadParameters(
                    keyParam,
                    MacSize(json.GetValue("ts").ToObject<int>()),
                    nonce,
                    adataBytes);

                var aes = new CcmBlockCipher(new AesFastEngine());
                aes.Init(false, ccm);

                var plainBytes = new byte[aes.GetOutputSize(cipherText.Length)];

                int res = aes.ProcessBytes(
                    cipherText,
                    0,
                    cipherText.Length,
                    plainBytes,
                    0);

                aes.DoFinal(plainBytes, res);
                var text = Encoding.UTF8.GetString(plainBytes);
                return JObject.Parse(text);
            }
            catch (InvalidCipherTextException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Json decryption failed.", e);
            }
        }

        private int MacSize(int ms)
        {
            return _ts;
        }

        private string EncodeAdata(string adata)
        {
            return JsEscape.Escape(adata);
        }

        private byte[] DecodeAdataBytes(string adata)
        {
            return Encoding.UTF8.GetBytes(JsEscape.Unescape(adata));
        }

        private KeyParameter CreateKey(string password, byte[] salt, int iterations, int keySizeInBits)
        {
            var generator = new Pkcs5S2ParametersGenerator(new Sha256Digest());
            generator.Init(PbeParametersGenerator.Pkcs5PasswordToUtf8Bytes(password.ToCharArray()), salt, iterations);

            return (KeyParameter)generator.GenerateDerivedMacParameters(keySizeInBits);
        }

        private byte[] ComputeNonce(byte[] iv, byte[] plainBytes)
        {
            int ivl = iv.Length;
            int ol = plainBytes.Length - (_ts / 8);
            int l = 2;

            while (l < 4 && (((uint)ol) >> 8 * l) != 0)
            {
                l++;
            }

            if (l < 15 - ivl)
            {
                l = 15 - ivl;
            }

            int newLength = 15 - l;

            var copy = new byte[newLength];
            Array.Copy(iv, 0, copy, 0,
                             Math.Min(iv.Length, newLength));
            return copy;
        }
    }
}
