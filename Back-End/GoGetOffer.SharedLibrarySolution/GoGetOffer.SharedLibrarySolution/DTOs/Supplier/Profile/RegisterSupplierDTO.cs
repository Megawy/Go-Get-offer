using Microsoft.AspNetCore.Http;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile
{
    public class RegisterSupplierDTO
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public List<IFormFile>? CommercialRegistrationDocuments { get; set; }
        public List<IFormFile>? TaxCardDocuments { get; set; }
        public List<string>? ActivityType { get; set; }

        public string? BranchName { get; set; }
        public string? Governorate { get; set; }
        public string? City { get; set; }
        public string? Area { get; set; }
        public string? AddressDetails { get; set; }
        public string? PostalCode { get; set; }
        public List<string>? PhoneNumbers { get; set; }
    }
}
