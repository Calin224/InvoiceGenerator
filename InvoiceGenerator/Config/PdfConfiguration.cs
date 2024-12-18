using System;
using InvoiceGenerator.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceGenerator.Config;

public class PdfConfiguration : IEntityTypeConfiguration<PdfDocument>
{
    public void Configure(EntityTypeBuilder<PdfDocument> builder)
    {
        builder.Property(x => x.FileName).HasColumnType("varbinary(max)");
    }
}
