namespace GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser
{
    public class RequestUserUpdateDTO
    {
        public Guid? Id { get; set; }
        public Guid? AuthenticationUserId { get; set; }
        public string? NewEmail { get; set; }
        public string? NewPhoneNumber { get; set; }
        public string? NewCompanyName { get; set; }
        public string? UserComment { get; set; }
        public string? AdminComment { get; set; }
        public string? IsApproved { get; set; }
        public DateTime? RequestedAt { get; set; }
        public DateTime? DecisionAt { get; set; }
    }
}
