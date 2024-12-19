using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InvoiceGenerator.Data;
using InvoiceGenerator.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private readonly DataContext _context = new DataContext(); 

        public User User { get; set; }

        public EditWindow(User user)
        {
            InitializeComponent();
            User = user;
            
            Username.Text = User.UserName;
            Address.Text = User.Address;
            Cif.Text = User.CIF;
            Phone.Text = User.PhoneNumber;
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            User.UserName = Username.Text;
            User.Address = Address.Text;
            User.CIF = Cif.Text;
            User.PhoneNumber = Phone.Text;

            _context.Users.Update(User);
            _context.SaveChanges();

            MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
