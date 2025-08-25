using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SongLib
{
    public static class Security
    {
        private static bool m_bUseSecurity = true;
        public static string iv = SongLib.Security.GetDafaultKey().Substring(0, 16);

        public static bool UseSecurity
        {
            set => SongLib.Security.m_bUseSecurity = value;
        }

        public static string GetDafaultKey()
        {
            return SongLib.Security.m_bUseSecurity ? "isnara011516is0115kyskdmlrna316p" : "";
        }

        public static string MD5(string _str)
        {
            if (!SongLib.Security.m_bUseSecurity)
                return _str;
            byte[] bytes = Encoding.UTF8.GetBytes(_str);
            try
            {
                byte[] hash = new MD5CryptoServiceProvider().ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte num in hash)
                    stringBuilder.Append(num.ToString("x2"));
                DebugHelper.Log(EDebugType.System, (object)stringBuilder.ToString());
                return stringBuilder.ToString();
            }
            catch
            {
                throw;
            }
        }

        public static byte[] MakeMD5Hash(string _key)
        {
            return new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(_key));
        }

        public static string Base64Encode(string _str)
        {
            return !SongLib.Security.m_bUseSecurity ? _str : Convert.ToBase64String(Encoding.UTF8.GetBytes(_str));
        }

        public static string Base64Decode(string _str)
        {
            return !SongLib.Security.m_bUseSecurity ? _str : Encoding.UTF8.GetString(Convert.FromBase64String(_str));
        }

        public static byte[] Encrypt3DEString(byte[] _value, byte[] _secret)
        {
            TripleDES tripleDes = (TripleDES)new TripleDESCryptoServiceProvider();
            tripleDes.Key = _secret;
            tripleDes.Mode = CipherMode.ECB;
            return tripleDes.CreateEncryptor().TransformFinalBlock(_value, 0, _value.Length);
        }

        public static byte[] Decrypt3DEString(byte[] _value, byte[] _secret)
        {
            TripleDES tripleDes = (TripleDES)new TripleDESCryptoServiceProvider();
            tripleDes.Key = _secret;
            tripleDes.Mode = CipherMode.ECB;
            return tripleDes.CreateDecryptor().TransformFinalBlock(_value, 0, _value.Length);
        }

        public static string AESEncrypt256(string _str, string _key)
        {
            if (!SongLib.Security.m_bUseSecurity)
                return _str;
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.KeySize = 256;
                rijndaelManaged.BlockSize = 128;
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.Key = Encoding.UTF8.GetBytes(_key);
                rijndaelManaged.IV = Encoding.UTF8.GetBytes(SongLib.Security.iv);
                ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                byte[] inArray = (byte[])null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream =
                           new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(_str);
                        cryptoStream.Write(bytes, 0, bytes.Length);
                    }

                    inArray = memoryStream.ToArray();
                }

                return Convert.ToBase64String(inArray);
            }
        }

        public static string AESDecrypt256(string _str, string _key)
        {
            if (!SongLib.Security.m_bUseSecurity)
                return _str;
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.KeySize = 256;
                rijndaelManaged.BlockSize = 128;
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.Key = Encoding.UTF8.GetBytes(_key);
                rijndaelManaged.IV = Encoding.UTF8.GetBytes(SongLib.Security.iv);
                ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor();
                byte[] bytes = (byte[])null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream =
                           new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        byte[] buffer = Convert.FromBase64String(_str);
                        cryptoStream.Write(buffer, 0, buffer.Length);
                    }

                    bytes = memoryStream.ToArray();
                }

                return Encoding.UTF8.GetString(bytes);
            }
        }

        public static string AESEncrypt128(string _str, string _key)
        {
            if (!SongLib.Security.m_bUseSecurity)
                return _str;
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] bytes1 = Encoding.Unicode.GetBytes(_str);
            byte[] bytes2 = Encoding.ASCII.GetBytes(_key.Length.ToString());
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(_key, bytes2);
            ICryptoTransform encryptor =
                rijndaelManaged.CreateEncryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytes1, 0, bytes1.Length);
            cryptoStream.FlushFinalBlock();
            byte[] array = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(array);
        }

        public static string AESDecrypt128(string _str, string _key)
        {
            if (!SongLib.Security.m_bUseSecurity)
                return _str;
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] buffer = Convert.FromBase64String(_str);
            byte[] bytes = Encoding.ASCII.GetBytes(_key.Length.ToString());
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(_key, bytes);
            ICryptoTransform decryptor =
                rijndaelManaged.CreateDecryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(buffer);
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] numArray = new byte[buffer.Length];
            int count = cryptoStream.Read(numArray, 0, numArray.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.Unicode.GetString(numArray, 0, count);
        }
    }
}