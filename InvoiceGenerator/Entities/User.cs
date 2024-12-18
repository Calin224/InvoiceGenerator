using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace InvoiceGenerator.Entities;

public class User
{
    public int Id { get; set; }
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string PasswordHash { get; set; } = string.Empty;
    [Required] public string CIF { get; set; }
    
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsLoggedOut { get; set; } = true;

    public ICollection<PdfDocument> Pdfs { get; } = new List<PdfDocument>();
}