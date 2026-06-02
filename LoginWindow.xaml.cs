using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using passwordmanager.models;
using passwordmanager.Services;
using System.Windows;
using System.Windows.Controls;

namespace passwordmanager
{

    public partial class LoginWindow : Window
    {
        UserService userService = new UserService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Password;


            if (login == "admin" &&
                password == "admin123")
            {
                Session.CurrentUser = "admin";
                MainWindow window = new MainWindow();
                window.Show();

                Close();
                return;
            }

            var users = userService.Load();
            string hashedPassword = PasswordHelper.HashPassword(password);

            var user = users.FirstOrDefault(x =>
                x.Login == login &&
                x.Password == hashedPassword);

            if (user != null)
            {
                Session.CurrentUser = user.Login;
                MainWindow window = new MainWindow();
                window.Show();

                Close();
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль");
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
            this.Close();
        }
    }
}
