using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewPort.xaml
    /// </summary>
    public partial class ViewPort : Page
    {
        public bool IsSender = false;

        public List<Genre> Genres = new List<Genre>
        {
            new Genre()
            { Title = "Драма" },
            new Genre()
            { Title = "Комедия" },
            new Genre()
            { Title = "Боевик" },
            new Genre()
            { Title = "Ужасы" },
            new Genre()
            { Title = "Фантастика" }
        };

        public ViewPort()
        {
            InitializeComponent();
            profileMenu.Visibility = Visibility.Hidden;
            searchBorder.Visibility = Visibility.Hidden;
            catalog.ItemsSource = Genres;
            catalog.Visibility = Visibility.Hidden;
        }

        private void MainGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox)
                IsSender = true;
            
            if (IsSender == false)
            {
                ChangeVisible(sender);
            }

            if (sender is Border)
                IsSender = false;
        }

        private void ChangeVisible(object control)
        {
            if (searchBorder.IsVisible && !(control is TextBlock))
            {
                searchBorder.Visibility = Visibility.Hidden;
                search.Text = "";
            }

            if (profileMenu.IsVisible && !(control is Button))
            {
                profileMenu.Visibility = Visibility.Hidden;
            }

            if (catalog.IsVisible && !(control is ListBox))
            {
                catalog.Visibility = Visibility.Hidden;
            }
        }


        private void ProfileIconClick(object sender, RoutedEventArgs e)
        {
            if (profileMenu.IsVisible)
                profileMenu.Visibility = Visibility.Hidden;
            else
                profileMenu.Visibility = Visibility.Visible;

            ChangeVisible(new Button());
        }


        #region Profile Menu Button Clicks
        private void ProfileClick(object sender, RoutedEventArgs e)
        {

        }

        private void MineBookmarksClick(object sender, RoutedEventArgs e)
        {

        }

        private void MineCommentsClick(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {

        }

        private void AccLeaveClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new LoginPage());
        }
        #endregion


        #region Search Bar Events
        private void SearchVisibleClick(object sender, RoutedEventArgs e)
        {
            if (searchBorder.IsVisible)
            {
                searchBorder.Visibility = Visibility.Hidden;
                search.Text = "";
            }
            else
            {
                searchBorder.Visibility = Visibility.Visible;
            }

            ChangeVisible(new TextBlock());
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {

        }

        private void SearchPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[а-яА-Яa-zA-Z.,0-9]").Success)
            {
                e.Handled = true;
            }
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text != "")
            {
                backText.Visibility = Visibility.Hidden;
            }
            else
            {
                backText.Visibility = Visibility.Visible;
            }
        }
        #endregion


        #region Catalog Events
        private void CatalogClick(object sender, RoutedEventArgs e)
        {
            if (catalog.IsVisible)
                catalog.Visibility = Visibility.Hidden;
            else
                catalog.Visibility = Visibility.Visible;

            ChangeVisible(new ListBox());
        }

        private void CatalogSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (catalog.SelectedIndex == 0)
            {
                MessageBox.Show("We Win");
            }
            else if (catalog.SelectedIndex == 1)
            {
                MessageBox.Show("We lose");
            }
        }
        #endregion
    }
}
