using System;
using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Entities;

public class PdfDocument
{
    public int Id { get; set; }
    [Required]
    public string FileName { get; set; } = string.Empty;
    public byte[] FileData { get; set; }
    public DateTime DateCreated { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
