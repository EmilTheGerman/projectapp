using passwordmanager.models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace passwordmanager
{
    public partial class CategoriesWindow : Window
    {
        private List<PasswordItem> items;

        public PasswordItem SelectedItem { get; set; }

        public CategoriesWindow(List<PasswordItem> data)
        {
            InitializeComponent();
            items = data;
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            string category = (sender as Button).Content.ToString();

            CategoryTitle.Text = category;

            CategoryList.ItemsSource = items
                .Where(x => x.Category == category)
                .ToList();
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryList.SelectedItem is PasswordItem selected)
            {
                SelectedItem = selected;
                DialogResult = true;
            }
        }
    }
}
