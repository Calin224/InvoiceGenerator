namespace InvoiceGenerator.DTOs;

public class UserDto
{
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string CIF { get; set; }
    
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
}