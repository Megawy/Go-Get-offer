using Microsoft.AspNetCore.Http;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile
{
    public class UpdateSupplierDTO
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public List<IFormFile>? CommercialRegistrationDocuments { get; set; }
        public List<IFormFile>? TaxCardDocuments { get; set; }
        public List<string>? ActivityType { get; set; }
    }
}
