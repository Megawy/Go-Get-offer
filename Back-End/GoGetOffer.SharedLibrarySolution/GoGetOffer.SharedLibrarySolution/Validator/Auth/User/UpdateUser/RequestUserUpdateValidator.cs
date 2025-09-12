using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;

namespace GoGetOffer.SharedLibrarySolution.Validator.Auth.User.UpdateUser
{
    public class RequestUserUpdateValidator : AbstractValidator<RequestUserUpdateDTO>
    {
        public RequestUserUpdateValidator()
        {
            RuleFor(x => x.NewEmail)
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(300).WithMessage("Email can't exceed 300 characters.");

            RuleFor(x => x.NewPhoneNumber)
                .Matches(@"^01[0-2,5]{1}[0-9]{8}$").WithMessage("Phone number is not valid.")
                .MaximumLength(30).WithMessage("Phone number can't exceed 30 characters.");

            RuleFor(x => x.NewCompanyName)
                .MinimumLength(3).WithMessage("CompanyName must be at least 3 characters.")
                .MaximumLength(300).WithMessage("CompanyName can't exceed 160 characters.");

            RuleFor(x => x.UserComment)
                .NotEmpty().WithMessage("UserComment is required.")
                .MinimumLength(5).WithMessage("UserComment must be at least 5 characters.")
                .MaximumLength(800).WithMessage("UserComment can't exceed 800 characters.");

            RuleFor(x => x.AdminComment)
                .MinimumLength(5).WithMessage("AdminComment must be at least 5 characters.")
                .MaximumLength(800).WithMessage("AdminComment can't exceed 800 characters.");
        }
    }
}
