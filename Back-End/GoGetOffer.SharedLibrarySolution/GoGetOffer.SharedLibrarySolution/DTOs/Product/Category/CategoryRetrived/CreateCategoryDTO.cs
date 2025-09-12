using Microsoft.AspNetCore.Http;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Product.Category.CategoryRetrived
{
    public class CreateCategoryDTO
    {
        public IFormFile? Img { get; set; }
        public List<CategoryTranslationDTO> Translations { get; set; } = new();
    }
    public class CategoryTranslationDTO
    {
        public string LanguageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
