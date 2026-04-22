using projectapp.models;
using projectapp.Services;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projectapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataService dataService = new DataService();
        List<PasswordItem> items;

        public MainWindow()
        {
            InitializeComponent();

            items = dataService.Load();
            PasswordList.ItemsSource = items;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var item = new PasswordItem
            {
                Title = TitleBox.Text,
                Login = LoginBox.Text,
                Password = PasswordBox.Password,
                Category = (CategoryBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString(),
                Description = DescriptionBox.Text
            };

            items.Add(item);
            PasswordList.Items.Refresh();
            dataService.Save(items);

            ClearFields();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordList.SelectedItem is PasswordItem selected)
            {
                items.Remove(selected);
                PasswordList.Items.Refresh();
                dataService.Save(items);

                ClearFields();
            }
        }

        private void PasswordList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PasswordList.SelectedItem is PasswordItem selected)
            {
                TitleBox.Text = selected.Title;
                LoginBox.Text = selected.Login;
                PasswordBox.Password = selected.Password;
                DescriptionBox.Text = selected.Description;
            }
        }
        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string text = SearchBox.Text.ToLower();

            PasswordList.ItemsSource = items
                .Where(x => x.Title.ToLower().Contains(text))
                .ToList();
        }
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            int length = (int)LengthSlider.Value;

            bool upper = UpperCheck.IsChecked == true;
            bool lower = LowerCheck.IsChecked == true;
            bool digits = DigitCheck.IsChecked == true;
            bool symbols = SymbolCheck.IsChecked == true;

            PasswordBox.Password = PasswordGenerator.Generate(length, upper, lower, digits, symbols);
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(PasswordBox.Password);
        }

        private void ClearFields()
        {
            TitleBox.Text = "";
            LoginBox.Text = "";
            PasswordBox.Password = "";
            DescriptionBox.Text = "";
            CategoryBox.SelectedIndex = -1;
        }
    }
}