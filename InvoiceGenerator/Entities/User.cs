using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace InvoiceGenerator.Entities;

public class User
{
    public int Id { get; set; }
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string PasswordHash { get; set; } = string.Empty;
}