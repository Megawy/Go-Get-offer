namespace GoGetOffer.SharedLibrarySolution.DTOs.Cloudinary
{
    public sealed class TransformOptionsDTO
    {
        public int? Width { get; init; }
        public int? Height { get; init; }
        public string? Crop { get; init; } // fill, fit, scale, thumb, crop
        public string? Gravity { get; init; } // auto, face, etc.
        public string? Background { get; init; } // for pad
        public string? Format { get; init; } // jpg, webp, mp4, etc.
        public string? Effect { get; init; } // e.g., "sharpen"
        public string? Quality { get; init; } // auto, 80, etc.
        public bool? FetchFormatAuto { get; init; }
        public bool? Progressive { get; init; }
    }
}
