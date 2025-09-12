using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;

namespace GoGetOffer.SharedLibrarySolution.Validator.Suppliers.Query
{
    public class CodeValidator : AbstractValidator<CodeSupplierDTO>
    {
        public CodeValidator()
        {
            RuleFor(x => x.code)
                .NotEmpty().WithMessage("code is required.");
        }
    }
}
