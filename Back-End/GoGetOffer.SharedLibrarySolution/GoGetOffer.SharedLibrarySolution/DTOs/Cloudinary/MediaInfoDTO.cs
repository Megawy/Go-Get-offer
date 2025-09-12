namespace GoGetOffer.SharedLibrarySolution.DTOs.Cloudinary
{
    public sealed class MediaInfoDTO
    {
        public string PublicId { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        public string SecureUrl { get; init; } = string.Empty;
        public string ResourceType { get; init; } = string.Empty; // image/video/raw
        public string Format { get; init; } = string.Empty;
        public long Bytes { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public DateTime CreatedAt { get; init; }
        public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();
        public string Version { get; init; } = string.Empty;
    }
}
