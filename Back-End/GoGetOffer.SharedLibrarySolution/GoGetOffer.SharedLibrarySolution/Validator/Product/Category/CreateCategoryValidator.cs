using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Product.Category.CategoryRetrived;

namespace GoGetOffer.SharedLibrarySolution.Validator.Product.Category
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Img)
              .NotNull().WithMessage("Img is required.");

        }
    }
    public class CategoryTranslationValidator : AbstractValidator<CategoryTranslationDTO>
    {
        public CategoryTranslationValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Name is required.");
        }
    }
}
