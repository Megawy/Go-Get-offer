namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile
{
    public class AddSomeDataSupplierDTO
    {
        public Guid? Id { get; set; }
        public int? MinProducts { get; set; }
        public int? MaxProducts { get; set; }
        public int? DeliveryTimeInDays { get; set; }
        public decimal? MinInvoiceAmount { get; set; }
        public bool HasElctroinInvoice { get; set; }
        public bool HasDeliveryService { get; set; }
    }
}
