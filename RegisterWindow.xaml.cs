using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using passwordmanager.models;
using passwordmanager.Services;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace passwordmanager
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Заповніть усі поля");
                return;
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(LoginBox.Text, emailPattern))
            {
                MessageBox.Show("Введіть коректну електронну пошту");
                return;
            }

            if (PasswordBox.Password.Length < 6)
            {
                MessageBox.Show("Пароль повинен містити мінімум 6 символів");
                return;
            }

            UserService service = new UserService();

            var users = service.Load();

            if (users.Any(x => x.Login == LoginBox.Text))
            {
                MessageBox.Show("Користувач існує");
                return;
            }

            users.Add(new User
            {
                Login = LoginBox.Text,
                Password = PasswordHelper.HashPassword(PasswordBox.Password),
                Role = "User"
            });

            service.Save(users);

            MessageBox.Show("Реєстрація успішна");

            LoginWindow login = new LoginWindow();
            login.Show();

            Close();
        }
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();

            this.Close();
        }
    }
}
