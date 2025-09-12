using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Register;

namespace GoGetOffer.SharedLibrarySolution.Validator.Auth.User.Register
{
    public class RegisterValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
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

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^01[0-2,5]{1}[0-9]{8}$").WithMessage("Phone number is not valid.")
                .MaximumLength(30).WithMessage("Phone number can't exceed 30 characters.");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("CompanyName is required.")
                .MinimumLength(3).WithMessage("CompanyName must be at least 3 characters.")
                .MaximumLength(300).WithMessage("CompanyName can't exceed 160 characters.");
        }
    }
}
