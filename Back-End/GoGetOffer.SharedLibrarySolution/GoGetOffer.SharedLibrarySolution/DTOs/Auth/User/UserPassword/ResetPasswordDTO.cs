namespace GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserPassword
{
    public class ResetPasswordDTO
    {
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
        public string? ResetCode { get; set; }
    }
}
