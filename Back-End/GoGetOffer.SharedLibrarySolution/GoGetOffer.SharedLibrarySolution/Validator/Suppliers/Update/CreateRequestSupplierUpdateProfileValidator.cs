using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;

namespace GoGetOffer.SharedLibrarySolution.Validator.Suppliers.Update
{
    public class CreateRequestSupplierUpdateProfileValidator : AbstractValidator<CreateRequestSupplierUpdateProfileDTO>
    {
        public CreateRequestSupplierUpdateProfileValidator()
        {
            RuleFor(x => x.UserComment)
               .NotNull().WithMessage("UserComment is required.");
        }
    }
}
