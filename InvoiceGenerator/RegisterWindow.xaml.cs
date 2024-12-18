using System.Windows;
using InvoiceGenerator.Data;
using InvoiceGenerator.Entities;
using MaterialDesignThemes.Wpf;

namespace InvoiceGenerator;

public partial class RegisterWindow : Window
{
    private readonly DataContext _context = new DataContext();

    public string RegisteredUsername { get; set; }
    
    public RegisterWindow()
    {
        InitializeComponent();
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        string username = Username.Text;
        string password = Password.Password;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Username and password are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User()
        {
            UserName = username,
            PasswordHash = hashedPassword
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        MessageBox.Show("Registration Succesfull!", "Success",MessageBoxButton.OK, MessageBoxImage.Information);
        // this.Close();
        
        RegisteredUsername = username;
        DialogResult = true;
        Close();
    }

    private void LoginRedirect_Click(object sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow();
        loginWindow.ShowDialog();
    }
}