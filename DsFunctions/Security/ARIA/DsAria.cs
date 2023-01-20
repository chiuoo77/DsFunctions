using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DsFunctions.Security.ARIA
{
    internal class DsAria
    {
        private static readonly int BLOCK_SIZE = 16;

        private ARIAEngine engine = null;

        public DsAria(byte[] key)
        {
            Init(key);
        }

        public DsAria(String key)
        {
            Init(CreateKey(key));
        }

        private void Init(byte[] key)
        {
            engine = new ARIAEngine(key.Length * 8);
            engine.SetKey(key);
            engine.SetupRoundKeys();
        }

        /// <summary>
        /// ARIA는 key 사이즈가 128, 192, 256 bit 중에 하나 이어야만 한다.
        /// 아래는 단순 MD5를 돌려서 128 bit 키를 만들어낸다. 이 부분을 필요에 따라
        /// 상속하여 문자열에서 적절한 key를 생성하도록 변경하면 된다. (그냥 써도 무방하다)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected byte[] CreateKey(string key)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] hashData = md5Hasher.ComputeHash(Encoding.Default.GetBytes(key));
            return hashData;
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] indata = PKCS5Padding.Pad(data, BLOCK_SIZE);
            byte[] outdata = new byte[indata.Length];

            for (int i = 0; i < indata.Length; i += BLOCK_SIZE)
            {
                engine.Encrypt(indata, i, outdata, i);
            }

            return outdata;
        }

        public byte[] Decrypt(byte[] data)
        {
            byte[] outdata = new byte[data.Length];

            for (int i = 0; i < data.Length; i += BLOCK_SIZE)
            {
                engine.Decrypt(data, i, outdata, i);
            }

            return PKCS5Padding.UnPad(outdata, BLOCK_SIZE);
        }

        public String EncryptString(string data, Encoding encoding)
        {
            byte[] bytes = Encrypt(encoding.GetBytes(data));
            return Convert.ToBase64String(bytes);
        }

        public String DecryptString(string data, Encoding encoding)
        {
            byte[] bytes = Decrypt(Convert.FromBase64String(data));
            if (bytes == null)
                return null;

            return encoding.GetString(bytes);
        }

        public String encryptString(String data)
        {
            if (data == null || data == string.Empty) return null;
            byte[] bytes = Encrypt(Encoding.Default.GetBytes(data));
            return Convert.ToBase64String(bytes);
        }

        public String decryptString(String data)
        {
            try
            {
                if (data == null || data == string.Empty) return null;
                byte[] bytes = Decrypt(Convert.FromBase64String(data));
                if (bytes == null)
                    return null;

                return Encoding.Default.GetString(bytes);
            }
            catch
            {
                return data;
            }
        }
    }
}
