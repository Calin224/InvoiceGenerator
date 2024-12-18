using System;
using InvoiceGenerator.Config;
using InvoiceGenerator.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Data;

public class DataContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=invoiceGenerator;User Id=SA;Password=Password@1;TrustServerCertificate=True");
    }

    public DbSet<PdfDocument> PdfDocuments { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PdfConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}
