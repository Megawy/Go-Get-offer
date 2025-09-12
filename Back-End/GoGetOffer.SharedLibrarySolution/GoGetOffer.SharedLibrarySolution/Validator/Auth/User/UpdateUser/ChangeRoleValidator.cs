using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;

namespace GoGetOffer.SharedLibrarySolution.Validator.Auth.User.UpdateUser
{
    public class ChangeRoleValidator : AbstractValidator<ChangeRoleDTO>
    {
        public ChangeRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("RoleName is required.")
                .MinimumLength(2).WithMessage("RoleName must be at least 2 characters.")
                .MaximumLength(80).WithMessage("RoleName can't exceed 80 characters.");
        }
    }
}
