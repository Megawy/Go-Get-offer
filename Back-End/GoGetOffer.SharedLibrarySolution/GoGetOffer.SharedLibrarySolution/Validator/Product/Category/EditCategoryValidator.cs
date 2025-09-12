using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Product.Category.CategoryRetrived;

namespace GoGetOffer.SharedLibrarySolution.Validator.Product.Category
{
    public class EditCategoryValidator : AbstractValidator<EditCategoryDTO>
    {
        public EditCategoryValidator()
        {
            RuleFor(x => x.Id)
              .NotNull().WithMessage("Id is required.");
        }
    }
}
