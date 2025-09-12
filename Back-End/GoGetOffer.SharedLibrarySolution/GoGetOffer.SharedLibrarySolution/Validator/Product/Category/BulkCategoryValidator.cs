using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Product.Category.CategoryRetrived;

namespace GoGetOffer.SharedLibrarySolution.Validator.Product.Category
{
    public class BulkCategoryValidator : AbstractValidator<BulkCategoryDTO>
    {
        public BulkCategoryValidator()
        {
            RuleFor(x => x.File)
             .NotNull().WithMessage("File is required.");
        }
    }
}
