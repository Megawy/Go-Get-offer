using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;

namespace GoGetOffer.SharedLibrarySolution.Validator.Suppliers.Branch
{
    public class CreateBranchValidator : AbstractValidator<CreateBranchDTO>
    {
        public CreateBranchValidator()
        {
            RuleFor(x => x.BranchName)
               .NotEmpty().WithMessage("BranchName is required.")
               .MinimumLength(3).WithMessage("BranchName must be at least 3 characters.");

            RuleFor(x => x.Governorate)
               .NotEmpty().WithMessage("Governorate is required.")
               .MinimumLength(3).WithMessage("Governorate must be at least 3 characters.");


            RuleFor(x => x.City)
               .NotEmpty().WithMessage("City is required.")
               .MinimumLength(3).WithMessage("City must be at least 3 characters.");


            RuleFor(x => x.Area)
               .NotEmpty().WithMessage("Area is required.")
               .MinimumLength(3).WithMessage("Area must be at least 3 characters.");


            RuleFor(x => x.AddressDetails)
               .NotEmpty().WithMessage("AddressDetails is required.")
               .MinimumLength(10).WithMessage("AddressDetails must be at least 10 characters.");


            RuleFor(x => x.PostalCode)
               .NotEmpty().WithMessage("PostalCode is required.")
               .MinimumLength(4).WithMessage("PostalCode must be at least 4 characters.");
        }
    }
}
