using System.Configuration;
using System.Data;
using System.Windows;
using InvoiceGenerator.Data;

namespace InvoiceGenerator;

public partial class App : Application
{
    private readonly DataContext _context = new DataContext();
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var activeUser = _context.Users.FirstOrDefault(u => u.IsLoggedOut == false);
        
        if (activeUser != null)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
        else
        {
            var loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}

