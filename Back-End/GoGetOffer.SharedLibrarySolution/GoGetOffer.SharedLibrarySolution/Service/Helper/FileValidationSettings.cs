namespace GoGetOffer.SharedLibrarySolution.Service.Helper
{
    public static class FileValidationSettings
    {
        public const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
        public static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
    }
}
