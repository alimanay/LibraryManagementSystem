using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;


namespace Kütüphane_Yonetim_Sistemi.Helpers
{
    public class EncryptionHelper
    {
        private static byte[] _keyBytes = Array.Empty<byte>();

        public static void SetKey(string key)
        {
            _keyBytes = Convert.FromBase64String(key);
        }
        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            using var aes = Aes.Create();
            aes.Key = _keyBytes;
            aes.IV = new byte[16];

            var encrytor = aes.CreateEncryptor();
            var bytes = Encoding.UTF8.GetBytes(text);
            var result = encrytor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(result);
        }

        public static string Decrypt(string encrypted)
        {
            try
            {

                if (string.IsNullOrEmpty(encrypted)) return encrypted;
                using var aes = Aes.Create();
                aes.Key = _keyBytes;
                aes.IV = new byte[16];
                var decryptor = aes.CreateDecryptor();
                var bytes = Convert.FromBase64String(encrypted);
                var result = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                return Encoding.UTF8.GetString(result);

            }
            catch { 
            
             return encrypted;
            }
            }
    }
}
