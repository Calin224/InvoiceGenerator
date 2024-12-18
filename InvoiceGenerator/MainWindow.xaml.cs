using System.IO;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;
using InvoiceGenerator.Data;
using InvoiceGenerator.Services;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Path = System.IO.Path;

namespace InvoiceGenerator;

public partial class MainWindow : Window
{
    private readonly DataContext _context = new DataContext();
    private readonly PdfServices _pdfServices = new PdfServices();

    public MainWindow()
    {
        InitializeComponent();
        LoadPdfs();
    }

    private void UpdateNoItems_OnClick(object sender, RoutedEventArgs e)
    {
        int noItems;
        if (!int.TryParse(NoItems.Text, out noItems))
        {
            MessageBox.Show("Please enter a valid number for 'NoItems'.", "Invalid Input", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        Form.Children.Clear();

        GenerateItemTextBoxes(noItems);
    }

    private void GenerateItemTextBoxes(int noItems)
    {
        for (int i = 0; i < noItems; i++)
        {
            StackPanel itemPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 10, 0, 10)
            };

            Label label = new Label
            {
                Content = $"Item {i + 1}:",
                VerticalAlignment = VerticalAlignment.Center,
                Width = 70
            };

            TextBox tbItem = new TextBox()
            {
                Name = $"Item_{i}",
                Style = (Style)FindResource("MaterialDesignOutlinedTextBox"),
                Margin = new Thickness(10)
            };
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tbItem, "Item name");
            MaterialDesignThemes.Wpf.HintAssist.SetIsFloating(tbItem, true);

            TextBox tbQuantity = new TextBox()
            {
                Name = $"Quantity_{i}",
                Style = (Style)FindResource("MaterialDesignOutlinedTextBox"),
                Margin = new Thickness(10)
            };
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tbQuantity, "Quantity");
            MaterialDesignThemes.Wpf.HintAssist.SetIsFloating(tbQuantity, true);

            TextBox tbUnitPrice = new TextBox()
            {
                Name = $"UnitPrice_{i}",
                Style = (Style)FindResource("MaterialDesignOutlinedTextBox"),
                Margin = new Thickness(10)
            };
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tbUnitPrice, "Unit price");
            MaterialDesignThemes.Wpf.HintAssist.SetIsFloating(tbUnitPrice, true);


            itemPanel.Children.Add(tbItem);
            itemPanel.Children.Add(tbQuantity);
            itemPanel.Children.Add(tbUnitPrice);

            Form.Children.Add(itemPanel);
        }
    }

    private void LoadPdfs()
    {
        var pdfs = _context.PdfDocuments.ToList();
        PdfListView.ItemsSource = pdfs;
    }

    private void GeneratePdf_Click(object sender, RoutedEventArgs e)
    {
        bool platitorTVA = false;
        
        string fileName = FileName.Text;
        string customerName = CustomerName.Text;
        string cui = Cui.Text;
        
        var items = new List<(string Item, double Quantity, double UnitPrice)>();

        for (int i = 0; i < Form.Children.Count; i++)
        {
            if (Form.Children[i] is StackPanel itemPanel)
            {
                var itemBox = itemPanel.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name.StartsWith("Item_"));
                var quantityBox = itemPanel.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name.StartsWith("Quantity_"));
                var unitPriceBox = itemPanel.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name.StartsWith("UnitPrice_"));

                if (itemBox == null || quantityBox == null || unitPriceBox == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(itemBox.Text) ||
                    !double.TryParse(quantityBox.Text, out double quantity) ||
                    !double.TryParse(unitPriceBox.Text, out double unitPrice))
                {
                    MessageBox.Show($"Invalid input for item {i + 1}", "Validation Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                
                items.Add((itemBox.Text, quantity, unitPrice));
            }
        }
        
        if (string.IsNullOrWhiteSpace(fileName))
        {
            MessageBox.Show("Please enter a file name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(customerName))
        {
            MessageBox.Show("Please enter a customer name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (items.Count == 0)
        {
            MessageBox.Show("Please enter at least one item.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!string.IsNullOrWhiteSpace(cui) && cui.Contains("RO"))
        {
            platitorTVA = true;
        }

        byte[] pdfBytes = _pdfServices.CreatePdf(customerName, cui, items, platitorTVA);
        
        SavePdf(pdfBytes, fileName);
        
        FileName.Clear();
        CustomerName.Clear();
        Form.Children.Clear();
        
        LoadPdfs();
    }

    private void SavePdf(byte[] data, string fileName)
    {
        var pdfDocument = new Entities.PdfDocument()
        {
            FileName = fileName,
            FileData = data,
            DateCreated = DateTime.Now
        };

        _context.PdfDocuments.Add(pdfDocument);
        _context.SaveChanges();
    }

    private void OpenPdf_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button == null) return;

        if (button.Tag is not int pdfId) return;

        var pdf = _context.PdfDocuments.FirstOrDefault(p => p.Id == pdfId);

        if (pdf == null) MessageBox.Show("Pdf not found!");

        string tempFilePath = Path.Combine(Path.GetTempPath(), pdf.FileName);

        try
        {
            File.WriteAllBytes(tempFilePath, pdf.FileData);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = tempFilePath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening PDF: {ex.Message}");
        }
    }

    private void DeletePdfButton_OnClick(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button == null)
            return;

        if (button.Tag is not int pdfId) return;

        var pdf = _context.PdfDocuments.FirstOrDefault(x => x.Id == pdfId);

        if (pdf == null)
            return;

        _context.Remove(pdf);
        _context.SaveChanges();

        LoadPdfs();

        MessageBox.Show("Pdf file deleted succesfully!");
    }
}