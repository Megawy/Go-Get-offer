using GoGetOffer.SharedLibrarySolution.DTOs.Cloudinary;

namespace GoGetOffer.SharedLibraySolution.DTOs.Cloudinary
{
    public enum MediaType
    {
        Image,
        Video,
        Raw // pdf, zip, etc.
    }
    public sealed class UploadRequestDTO
    {
        public string? FileName { get; init; }
        public string? Folder { get; init; }
        public MediaType Type { get; init; } = MediaType.Image;
        public IEnumerable<string>? Tags { get; init; }
        public TransformOptionsDTO? Transform { get; init; }
        public bool Overwrite { get; init; } = false;
        public string? PublicId { get; init; }
        public Stream? FileStream { get; set; }
    }
}
