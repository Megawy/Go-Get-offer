using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.ConfirmEmail;

namespace GoGetOffer.SharedLibrarySolution.Validator.Auth.User.ConfirmEmail
{
    public class ConfirmEmailOtpValidator : AbstractValidator<ConfirmEmailOtpDTO>
    {
        public ConfirmEmailOtpValidator()
        {
            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("Otp is required.")
                .MaximumLength(6).WithMessage("Otp can't exceed 6 characters.");
        }
    }
}
