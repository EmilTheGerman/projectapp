using passwordmanager.models;
using passwordmanager.Services;
using System.Text.RegularExpressions;
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

namespace passwordmanager
{
    public partial class MainWindow : Window
    {
        DataService dataService = new DataService();
        List<PasswordItem> items;
        PasswordItem selectedItem = null;


        public MainWindow()
        {
            InitializeComponent();

            if (Session.CurrentUser == "admin")
            {
                items = dataService.Load();
            }
            else
            {
                items = dataService.Load()
                    .Where(x => x.Owner == Session.CurrentUser)
                    .ToList();
            }
            PasswordList.ItemsSource = items;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Введіть логін і пароль!");
                return;
            }

            if (!IsValidEmail(LoginBox.Text))
            {
                MessageBox.Show("Логін має бути у форматі email!");
                return;
            }

            string category = (CategoryBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (currentItem == null)
            {
                currentItem = new PasswordItem();
                items.Add(currentItem);
            }

            currentItem.Owner = Session.CurrentUser;
            currentItem.Title = TitleBox.Text;
            currentItem.Login = LoginBox.Text;

            if (isPasswordVisible)
            {
                PasswordBox.Password = PasswordTextBox.Text;
            }
            else
            {
                PasswordTextBox.Text = PasswordBox.Password;
            }

            string password = isPasswordVisible
    ? PasswordTextBox.Text
    : PasswordBox.Password;

            currentItem.Password =
                PasswordHelper.Encrypt(password);

            currentItem.Category =
                (CategoryBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            currentItem.Description = DescriptionBox.Text;

            var allItems = dataService.Load();

            if (!allItems.Contains(currentItem))
            {
                allItems.Add(currentItem);
            }
            else
            {
                int index = allItems.FindIndex(x =>
                    x.Title == currentItem.Title &&
                    x.Owner == currentItem.Owner);

                if (index >= 0)
                    allItems[index] = currentItem;
            }

            dataService.Save(allItems);

            PasswordList.Items.Refresh();

            DetailsPanel.Visibility = Visibility.Collapsed;

            ClearFields();
            PasswordBox.Password = "";
            PasswordTextBox.Text = "";
        }
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
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
            DetailsPanel.Visibility = Visibility.Collapsed;
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility =
            string.IsNullOrWhiteSpace(SearchBox.Text)
            ? Visibility.Visible
            : Visibility.Collapsed;
            string text = SearchBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(text))
            {
                PasswordList.ItemsSource = items;
            }
            else
            {
                PasswordList.ItemsSource = items
                    .Where(x => x.Title.ToLower().Contains(text))
                    .ToList();
            }
        }
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            int length = (int)LengthSlider.Value;

            string chars = "";

            if (UpperCheck.IsChecked == true) chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (LowerCheck.IsChecked == true) chars += "abcdefghijklmnopqrstuvwxyz";
            if (DigitCheck.IsChecked == true) chars += "0123456789";
            if (SymbolCheck.IsChecked == true) chars += "!@#$%^&*()";

            if (chars == "")
            {
                MessageBox.Show("Оберіть хоча б один тип символів!");
                return;
            }

            Random rnd = new Random();

            string password = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());

            GeneratedPasswordBox.Text = password;
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GeneratedPasswordBox.Text);
        }

        private void ClearFields()
        {
            TitleBox.Text = "";
            LoginBox.Text = "";
            PasswordBox.Password = "";
            DescriptionBox.Text = "";
            CategoryBox.SelectedIndex = -1;
        }

        private void ShowGenerator_Click(object sender, RoutedEventArgs e)
        {
            GeneratorWindow win = new GeneratorWindow();

            if (win.ShowDialog() == true)
            {
                PasswordBox.Password = win.GeneratedPassword;
            }
        }
        private void PasswordList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PasswordList.SelectedItem is not PasswordItem selected)
                return;

            selectedItem = selected;

            TitleBox.Text = selected.Title;
            LoginBox.Text = selected.Login;
            string decryptedPassword = PasswordHelper.Decrypt(selected.Password);

            PasswordBox.Password = decryptedPassword;
            PasswordTextBox.Text = decryptedPassword;
            DescriptionBox.Text = selected.Description;

            foreach (ComboBoxItem item in CategoryBox.Items)
            {
                if (item.Content.ToString() == selected.Category)
                {
                    CategoryBox.SelectedItem = item;
                    break;
                }
            }
            DetailsPanel.Visibility = Visibility.Visible;
            currentItem = selected;
        }
        private bool isPasswordVisible = false;

        private void NewRecord_Click(object sender, RoutedEventArgs e)
        {
            currentItem = null;

            ClearFields();

            isPasswordVisible = false;

            PasswordBox.Visibility = Visibility.Visible;
            PasswordTextBox.Visibility = Visibility.Collapsed;
            DetailsPanel.Visibility = Visibility.Visible;
        }
        private void CloseDetails_Click(object sender, RoutedEventArgs e)
        {
            DetailsPanel.Visibility = Visibility.Collapsed;
        }
        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                PasswordBox.Password = PasswordTextBox.Text;

                PasswordBox.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordTextBox.Text = PasswordBox.Password;

                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Visible;
            }

            isPasswordVisible = !isPasswordVisible;
        }
        private void ShowCategories_Click(object sender, RoutedEventArgs e)
        {
            CategoriesWindow win = new CategoriesWindow(items);

            if (win.ShowDialog() == true)
            {
                PasswordList.SelectedItem = win.SelectedItem;
                PasswordList.ScrollIntoView(win.SelectedItem);
            }
        }

        private void ShowMain_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Visibility = Visibility.Visible;
            GeneratorPanel.Visibility = Visibility.Collapsed;
            CategoriesPanel.Visibility = Visibility.Collapsed;
            SettingsPanel.Visibility = Visibility.Collapsed;
        }
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            string category = (sender as Button).Content.ToString();

            CategoryTitle.Text = category;

            var filtered = items
                .Where(x => x.Category != null &&
                            x.Category.Trim() == category.Trim())
                .ToList();

            CategoryList.ItemsSource = filtered;
        }
        private void CategoryList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CategoryList.SelectedItem is PasswordItem selected)
            {
                ShowMain_Click(null, null);

                PasswordList.SelectedItem = selected;
                PasswordList.ScrollIntoView(selected);

                selectedItem = selected;

                TitleBox.Text = selected.Title;
                LoginBox.Text = selected.Login;
                PasswordBox.Password = selected.Password;
                DescriptionBox.Text = selected.Description;

                foreach (ComboBoxItem item in CategoryBox.Items)
                {
                    if (item.Content.ToString() == selected.Category)
                    {
                        CategoryBox.SelectedItem = item;
                        CategoryList.SelectedItem = null;
                        break;
                    }
                }
            }
            PasswordList.ItemsSource = items;
        }
        private void ShowSettings_Click(object sender, RoutedEventArgs e)
        {
            MainPanel.Visibility = Visibility.Collapsed;
            GeneratorPanel.Visibility = Visibility.Collapsed;
            CategoriesPanel.Visibility = Visibility.Collapsed;
            SettingsPanel.Visibility = Visibility.Visible;
        }
        private void ApplySettings_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme();
        }
        private void ApplyTheme()
        {
            string theme = (ThemeBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            var dict = new ResourceDictionary();

            if (theme == "Темна")
                dict.Source = new Uri("Resources/DarkTheme.xaml", UriKind.Relative);
            else
                dict.Source = new Uri("Resources/LightTheme.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Session.CurrentUser = null;

            LoginWindow login = new LoginWindow();
            login.Show();

            Close();
        }
        private PasswordItem currentItem;
    }
}