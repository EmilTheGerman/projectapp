using System;
using System.Linq;
using System.Windows;

namespace passwordmanager
{
    public partial class GeneratorWindow : Window
    {
        public string GeneratedPassword { get; set; }

        public GeneratorWindow()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            int length = (int)LengthSlider.Value;
            string chars = "";

            if (UpperCheck.IsChecked == true) chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (LowerCheck.IsChecked == true) chars += "abcdefghijklmnopqrstuvwxyz";
            if (DigitCheck.IsChecked == true) chars += "0123456789";
            if (SymbolCheck.IsChecked == true) chars += "!@#$%^&*";

            if (chars == "")
            {
                MessageBox.Show("Оберіть тип символів");
                return;
            }

            Random rnd = new Random();

            GeneratedPassword = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());

            ResultBox.Text = GeneratedPassword;
        }

        private void Use_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
