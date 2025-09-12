using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserPassword;

namespace GoGetOffer.SharedLibrarySolution.Validator.Auth.User.UserPassword
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(300).WithMessage("Email can't exceed 300 characters.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=\{\}\[\]\:\;\<\>\,\.\?\/\\]+")
                .WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ResetCode)
               .NotEmpty().WithMessage("ResetCode is required.");
        }
    }
}
