using System.Configuration;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace InvoiceGenerator.Services;

public class PdfServices
{
    private int InvoiceNumber { get; set; } = 0;

    private string test = "test";
    
    public byte[] CreatePdf(string customerName, string cui, string adresa, List<(string Item, double Quantity, double UnitPrice)> items, bool platitorTVA)
    {
        InvoiceNumber++;
        PdfDocument document = new PdfDocument();
        document.Info.Title = "Invoice PDF";

        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont titleFont = new XFont("Arial", 14, XFontStyleEx.Bold);
        XFont headerFont = new XFont("Arial", 12, XFontStyleEx.Bold);
        XFont regularFont = new XFont("Arial", 12);

        double yPosition = 50;

        gfx.DrawString("Factura proforma", titleFont, XBrushes.Black, new XPoint(100, yPosition));
        yPosition += 20;
        gfx.DrawString($"Invoice Number: {InvoiceNumber}", regularFont, XBrushes.Black, new XPoint(100, yPosition));
        yPosition += 20;
        gfx.DrawString($"Data: {DateTime.UtcNow.ToString().Substring(0, 10)}", titleFont, XBrushes.Black, new XPoint(100, yPosition));
        

        yPosition += 40;
        gfx.DrawString("Furnizor", regularFont, XBrushes.Black, new XPoint(50, yPosition));
        gfx.DrawString("Client", regularFont, XBrushes.Black, new XPoint(350, yPosition));
        DrawLine(gfx, ref yPosition);
        
        
        // coloana pentru furnizor
        gfx.DrawString($"SANDU CALIN MIHAI PFA", regularFont, XBrushes.Black, new XPoint(50, yPosition));
        yPosition += 20;
        gfx.DrawString($"241415", regularFont, XBrushes.Black, new XPoint(50, yPosition));
        yPosition -= 20;
        
        //coloana pentru client
        gfx.DrawString($"{customerName}", regularFont, XBrushes.Black, new XPoint(350, yPosition));
        yPosition += 20;
        gfx.DrawString($"CIF {cui}", regularFont, XBrushes.Black, new XPoint(350, yPosition));
        yPosition += 20;
        gfx.DrawString($"{adresa}", regularFont, XBrushes.Black, new XPoint(350, yPosition));
        yPosition -= 20;
        
        yPosition += 90;
        
        gfx.DrawString("Produs", headerFont, XBrushes.Black, new XPoint(50, yPosition));
        gfx.DrawString("Cantitate", headerFont, XBrushes.Black, new XPoint(150, yPosition));
        gfx.DrawString("Pret unitar fara TVA", headerFont, XBrushes.Black, new XPoint(250, yPosition));
        gfx.DrawString("Valoare fara TVA", headerFont, XBrushes.Black, new XPoint(450, yPosition));
        
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

        DrawLine(gfx, ref yPosition);
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


    private void DrawLine(XGraphics gfx, ref double yPos)
    {
        yPos += 20;
        gfx.DrawLine(XPens.Black, 50, yPos, 550, yPos);
        yPos += 20;
    }
}