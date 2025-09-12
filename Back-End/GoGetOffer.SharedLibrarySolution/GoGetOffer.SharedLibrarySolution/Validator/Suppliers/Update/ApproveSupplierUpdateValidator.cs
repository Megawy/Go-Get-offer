using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;

namespace GoGetOffer.SharedLibrarySolution.Validator.Suppliers.Update
{
    public class ApproveSupplierUpdateValidator : AbstractValidator<ApproveSupplierUpdateDTO>
    {
        public ApproveSupplierUpdateValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id is required.");

            RuleFor(x => x.Status)
                .NotNull().WithMessage("Status is required.");

            RuleFor(x => x.AdminComment)
                .NotNull().WithMessage("AdminComment is required.");
        }
    }
}
