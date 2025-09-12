using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserPassword;

namespace GoGetOffer.SharedLibrarySolution.Validator.Auth.User.UserPassword
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("NewPassword is required.")
                .MinimumLength(6).WithMessage("NewPassword must be at least 6 characters.")
                .Matches(@"[A-Z]+").WithMessage("NewPassword must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("NewPassword must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("NewPassword must contain at least one number.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=\{\}\[\]\:\;\<\>\,\.\?\/\\]+")
                .WithMessage("NewPassword must contain at least one special character.");

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("OldPassword is required.")
                .MinimumLength(6).WithMessage("OldPassword must be at least 6 characters.")
                .Matches(@"[A-Z]+").WithMessage("OldPassword must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("OldPassword must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("OldPassword must contain at least one number.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=\{\}\[\]\:\;\<\>\,\.\?\/\\]+")
                .WithMessage("OldPassword must contain at least one special character.");
        }
    }
}
