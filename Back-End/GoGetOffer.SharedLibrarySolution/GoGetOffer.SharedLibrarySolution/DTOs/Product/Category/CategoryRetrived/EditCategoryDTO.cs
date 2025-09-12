using Microsoft.AspNetCore.Http;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Product.Category.CategoryRetrived
{
    public class EditCategoryDTO
    {
        public Guid Id { get; set; }
        public IFormFile? Img { get; set; }
        public List<CategoryTranslationDTO> Translations { get; set; } = new();
    }
}
