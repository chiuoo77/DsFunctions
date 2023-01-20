using DsFunctions.Security.ARIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DsFunctions.Security
{
    public class DsCrypto
    {
        public string SecretKey { get; set; }

        public DsCrypto(string _SecretKey)
        {
            SecretKey = _SecretKey;
        }

        // SHA256 Hash
        public string SHA256Hash(String input)
        {
            if (input == "") return input;
            SHA256Managed sha256 = new SHA256Managed();
            String output = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(input)));
            return output;
        }

        // base64 + Sha256 Hash
        public string CalculateHash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// ARIA-128bit 암호화
        /// </summary>
        /// <param name="sPlaintext">평문</param>
        /// <param name="sMasterKey">키 문자열</param>
        /// <returns></returns>
        public string ARIAEncrypt128(string sPlaintext)
        {
            if (sPlaintext == "" || sPlaintext == null) return sPlaintext;
            if (SecretKey == "" || SecretKey == null) return sPlaintext;

            DsAria aria = new DsAria(SecretKey);
            return aria.encryptString(sPlaintext);
        }

        public string ARIADecrypt128(string sEncText)
        {
            if (sEncText == "" || sEncText == null) return sEncText;
            if (SecretKey == "" || SecretKey == null) return sEncText;

            DsAria aria = new DsAria(SecretKey);
            return aria.decryptString(sEncText);
        }

        // AES_256 암호화
        public string AESEncrypt256(string input)
        {
            if (input == "" || input == null) return input;
            if (SecretKey == "" || SecretKey == null) return input;

            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(SecretKey);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            String Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        public string Base64Encode(string input)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string input)
        {
            var base64EncodedBytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        //AES_256 복호화
        public string AESDecrypt256(string Input)
        {
            if (Input == "" || Input == null) return Input;
            if (SecretKey == "" || SecretKey == null) return Input;

            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(SecretKey);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            String Output = Encoding.UTF8.GetString(xBuff);
            return Output;
        }
    }
}
