namespace GoGetOffer.SharedLibrarySolution.Interface.Helper
{
    public interface IAesEncryptionHelperService
    {
        byte[] Encrypt(byte[] plainBytes);
        byte[] Decrypt(byte[] cipherBytes);
        string EncryptString(string plainText);
        string DecryptString(string cipherText);
    }
}
