using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserPassword;

namespace GoGetOffer.SharedLibrarySolution.Validator.Auth.User.UserPassword
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDTO>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(300).WithMessage("Email can't exceed 300 characters.");
        }
    }
}
