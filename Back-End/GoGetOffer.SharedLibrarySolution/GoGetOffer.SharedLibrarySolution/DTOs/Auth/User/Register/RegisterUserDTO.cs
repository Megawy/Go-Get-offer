namespace GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Register;

public class RegisterUserDTO
{
    public string? Email { get; set; }
    public string? CompanyName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PasswordHash { get; set; }
}
