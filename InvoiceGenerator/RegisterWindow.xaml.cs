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
        string confirmPassword = ConfirmPassword.Password;
        string cif = CIF.Text;
        string address = Adresa.Text;
        string telefon = NumarTelefon.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Username and password are required.", "Validation Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        if (password == confirmPassword)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User()
            {
                UserName = username,
                PasswordHash = hashedPassword,
                CIF = cif,
                PhoneNumber = telefon,
                Address = address
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            MessageBox.Show("Registration Succesfull!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            // this.Close();

            RegisteredUsername = username;
            DialogResult = true;
            Close();
        }
        else
        {
            MessageBox.Show("The passwords are not the same!", "Verification error!", MessageBoxButton.OK, MessageBoxImage.Error);
            ConfirmPassword.Focus();
        }
    }

    private void LoginRedirect_Click(object sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow();
        loginWindow.ShowDialog();
    }
}