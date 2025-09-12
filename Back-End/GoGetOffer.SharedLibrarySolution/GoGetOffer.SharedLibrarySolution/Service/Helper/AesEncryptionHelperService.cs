using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace GoGetOffer.SharedLibrarySolution.Service.Helper
{
    public class AesEncryptionHelperService : IAesEncryptionHelperService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public class EncryptionSettings
        {
            public string Key { get; set; } = string.Empty;
            public string IV { get; set; } = string.Empty;
        }


        public AesEncryptionHelperService(IOptions<EncryptionSettings> options)
        {
            var settings = options.Value;

            _key = Encoding.UTF8.GetBytes(settings.Key);
            _iv = Encoding.UTF8.GetBytes(settings.IV);
        }

        public byte[] Encrypt(byte[] plainBytes)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public byte[] Decrypt(byte[] cipherBytes)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        }

        public string EncryptString(string plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = Encrypt(plainBytes);
            return Convert.ToBase64String(cipherBytes);
        }

        public string DecryptString(string cipherText)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);
            var plainBytes = Decrypt(cipherBytes);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
