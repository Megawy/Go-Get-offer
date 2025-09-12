using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;

namespace GoGetOffer.SharedLibrarySolution.Validator.Auth.User.Login
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(300).WithMessage("Email can't exceed 300 characters.");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=\{\}\[\]\:\;\<\>\,\.\?\/\\]+")
                .WithMessage("Password must contain at least one special character.");
        }
    }
}
