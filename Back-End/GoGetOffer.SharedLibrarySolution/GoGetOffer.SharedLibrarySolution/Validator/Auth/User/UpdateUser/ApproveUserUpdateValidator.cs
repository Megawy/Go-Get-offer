using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;

namespace GoGetOffer.SharedLibrarySolution.Validator.Auth.User.UpdateUser
{
    public class ApproveUserUpdateValidator : AbstractValidator<ApproveUserUpdateDTO>
    {
        public ApproveUserUpdateValidator()
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
