namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest
{
    public class CreateRequestSupplierUpdateProfileDTO
    {
        public Guid UserId { get; set; }
        public string? NewFullName { get; set; }
        public string? NewGovernorate { get; set; }
        public string? NewCity { get; set; }
        public string? NewArea { get; set; }
        public string? NewAddressDetails { get; set; }
        public string? NewPostalCode { get; set; }
        public string? UserComment { get; set; }
    }
}
