using System.Windows;

namespace InvoiceGenerator;

public partial class LoginWindow : Window
{
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
        return username == "admin" && password == "admin";
    }


    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}