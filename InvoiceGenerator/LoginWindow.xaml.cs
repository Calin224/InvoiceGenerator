using System.Windows;
using InvoiceGenerator.Data;

namespace InvoiceGenerator;

public partial class LoginWindow : Window
{
    private readonly DataContext _context = new DataContext();

    public string CurrentUsername { get; set; }
    
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        string username = Username.Text;
        string password = Password.Password;

        if (ValidateLogin(username, password))
        {
            CurrentUsername = username;
            DialogResult = true;
            Close();
        }
        else
        {
            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private bool ValidateLogin(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == username);
        if (user == null) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
    
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        var registerWindow = new RegisterWindow();
        if (registerWindow.ShowDialog() == true)
        {
            Username.Text = registerWindow.RegisteredUsername;
            Password.Focus();
        }
        // registerWindow.ShowDialog();
    }
}