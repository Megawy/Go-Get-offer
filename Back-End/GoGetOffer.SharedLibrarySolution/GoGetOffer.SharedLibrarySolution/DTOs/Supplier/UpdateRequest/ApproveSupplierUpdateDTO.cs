namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest
{
    public class ApproveSupplierUpdateDTO
    {
        public Guid Id { get; set; }
        public string? Status { get; set; }
        public string? AdminComment { get; set; }
    }
}
