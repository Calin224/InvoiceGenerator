using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace InvoiceGenerator.Services;

public class PdfServices
{
    private int InvoiceNumber { get; set; } = 0;

    private string test = "test";
    
    public byte[] CreatePdf(string customerName, string cui, List<(string Item, double Quantity, double UnitPrice)> items, bool platitorTVA)
    {
        InvoiceNumber++;
        PdfDocument document = new PdfDocument();
        document.Info.Title = "Invoice PDF";

        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont titleFont = new XFont("Arial", 16, XFontStyleEx.Bold);
        XFont headerFont = new XFont("Arial", 12, XFontStyleEx.Bold);
        XFont regularFont = new XFont("Arial", 12);

        double yPosition = 50;

        gfx.DrawString("Factura proforma", titleFont, XBrushes.Black, new XPoint(200, yPosition));
        yPosition += 40;
        // gfx.DrawString($"{test}", titleFont, XBrushes.Black, new XPoint(200, yPosition));
        // yPosition += 40;

        gfx.DrawString($"Customer Name: {customerName}", regularFont, XBrushes.Black, new XPoint(50, yPosition));
        yPosition += 20;
        gfx.DrawString($"CIF: {cui}", regularFont, XBrushes.Black, new XPoint(50, yPosition));
        yPosition += 20;
        gfx.DrawString($"Invoice Date: {DateTime.UtcNow}", regularFont, XBrushes.Black, new XPoint(350, yPosition));
        yPosition += 20;
        gfx.DrawString($"Invoice Number: {InvoiceNumber}", regularFont, XBrushes.Black, new XPoint(50, yPosition));
        yPosition += 40;

        gfx.DrawString("Produs", headerFont, XBrushes.Black, new XPoint(50, yPosition));
        gfx.DrawString("Cantitate", headerFont, XBrushes.Black, new XPoint(150, yPosition));
        gfx.DrawString("Pret unitar fara TVA", headerFont, XBrushes.Black, new XPoint(250, yPosition));
        gfx.DrawString("Valoare fara TVA", headerFont, XBrushes.Black, new XPoint(350, yPosition));
        
        yPosition += 20;

        double totalAmount = 0;

        double quantity_no = 0;
        
        foreach (var item in items)
        {
            double itemTotal = item.Quantity * item.UnitPrice;
            totalAmount += itemTotal;

            quantity_no += item.Quantity;

            gfx.DrawString(item.Item, regularFont, XBrushes.Black, new XPoint(50, yPosition));
            gfx.DrawString($"{item.Quantity}", regularFont, XBrushes.Black, new XPoint(150, yPosition));
            gfx.DrawString(item.UnitPrice.ToString("C"), regularFont, XBrushes.Black, new XPoint(250, yPosition));
            gfx.DrawString(itemTotal.ToString("C"), regularFont, XBrushes.Black, new XPoint(450, yPosition));
            
            yPosition += 20;
        }

        yPosition += 20;
        gfx.DrawLine(XPens.Black, 50, yPosition, 500, yPosition);
        yPosition += 20;
        gfx.DrawString("Valoare fara TVA:", headerFont, XBrushes.Black, new XPoint(350, yPosition));
        gfx.DrawString($"{totalAmount.ToString("C")}", headerFont, XBrushes.Black, new XPoint(470, yPosition));
        yPosition += 20;

        if (platitorTVA)
        {
            double totalAmountPerQuantity = totalAmount / quantity_no;
            double TVA = totalAmount + totalAmount * 0.19;
            
            gfx.DrawString($"Valoare cu TVA: ", headerFont, XBrushes.Black, new XPoint(350, yPosition));
            gfx.DrawString($"{TVA.ToString("C")}", headerFont, XBrushes.Black, new XPoint(470, yPosition));
        }
        
        using (MemoryStream ms = new MemoryStream())
        {
            document.Save(ms);
            return ms.ToArray();
        }
    } 
}