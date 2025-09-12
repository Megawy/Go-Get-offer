namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch
{
    public class UpdateBranchDTO
    {
        public Guid? Id { get; set; }
        public string? BranchName { get; set; }
        public string? Governorate { get; set; }
        public string? City { get; set; }
        public string? Area { get; set; }
        public string? AddressDetails { get; set; }
        public string? PostalCode { get; set; }
        public List<string>? PhoneNumbers { get; set; }
    }
}
