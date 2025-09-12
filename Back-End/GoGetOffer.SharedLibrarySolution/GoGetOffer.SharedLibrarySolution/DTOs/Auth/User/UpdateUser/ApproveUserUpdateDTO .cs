namespace GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser
{
    public class ApproveUserUpdateDTO
    {
        public Guid Id { get; set; }
        public string? Status { get; set; }
        public string? AdminComment { get; set; }
    }
}
