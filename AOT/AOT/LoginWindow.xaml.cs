using AOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AOT
{
    /// <summary>
    /// Interaktionslogik für LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public bool IsAuthenticated { get; private set; } = false;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            DatabaseService db = new DatabaseService();
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            User user = await db.GetUserByUserName(username);

            AuthState.UserName = user.username;
            AuthState.Role = user.role;

            if (password == user.password) // Replace with real logic
            {
                IsAuthenticated = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid login");
            }
        }
    }
}
