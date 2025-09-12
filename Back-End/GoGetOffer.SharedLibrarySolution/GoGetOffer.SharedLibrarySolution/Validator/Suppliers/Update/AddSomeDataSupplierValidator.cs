using FluentValidation;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;

namespace GoGetOffer.SharedLibrarySolution.Validator.Suppliers.Update
{
    public class AddSomeDataSupplierValidator : AbstractValidator<AddSomeDataSupplierDTO>
    {
        public AddSomeDataSupplierValidator()
        {
            RuleFor(x => x.MinProducts)
                .NotNull().WithMessage("MinProducts is required.");

            RuleFor(x => x.DeliveryTimeInDays)
            .NotNull().WithMessage("DeliveryTimeInDays is required.");

            RuleFor(x => x.MinInvoiceAmount)
            .NotNull().WithMessage("MinInvoiceAmount is required.");

            RuleFor(x => x.HasElctroinInvoice)
            .NotNull().WithMessage("HasElctroinInvoice is required.");

            RuleFor(x => x.HasDeliveryService)
            .NotNull().WithMessage("HasDeliveryService is required.");
        }
    }
}
