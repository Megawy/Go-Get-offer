using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;

namespace GoGetOffer.SharedLibrarySolution.Validator.Suppliers.JoinRequest
{
    public class ReplyRequestJoinValidator : AbstractValidator<ReplyRequestJoinDTO>
    {
        public ReplyRequestJoinValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.IsApproved)
                .NotEmpty().WithMessage("IsApproved is required.")
                .MinimumLength(3).WithMessage("IsApproved must be at least 3 characters.");
        }
    }
}
