using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs;

namespace GoGetOffer.SharedLibrarySolution.Validator
{
    public class IDValidator : AbstractValidator<IDDTO>
    {
        public IDValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}



