using MessagePack;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Product.Category.CategoryRetrived
{
    [MessagePackObject]
    public class CategoryRetrivedDTO
    {
        [Key(0)]
        public Guid? Id { get; set; }
        [Key(1)]
        public string? ImgUrl { get; set; }
        [Key(2)]
        public string? ImgPublicId { get; set; }
        [Key(3)]
        public string? Name { get; set; }
    }
}
