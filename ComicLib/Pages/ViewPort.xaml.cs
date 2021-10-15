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
using ComicLib.ClassHelpers;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewPort.xaml
    /// </summary>
    public partial class ViewPort : System.Windows.Controls.Page
    {
        public bool IsSender = false;

        public ViewPort()
        {
            InitializeComponent();

            viewFrame.Content = new ComicListPage();

            DataContext = DBHelper.CurrentUser;

            profileMenu.Visibility = Visibility.Hidden;
            searchBorder.Visibility = Visibility.Hidden;
            catalog.ItemsSource = DBHelper.DBContext.Genre.ToList();
            catalog.Visibility = Visibility.Hidden;
            editComic.Visibility = Visibility.Hidden;

            numOfComics.Text = DBHelper.CurrentUser.Comic.Count.ToString();
        }

        private void MainGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox)
                IsSender = true;
            
            if (IsSender == false)
            {
                ChangeVisible(0);
            }

            if (sender is Border)
                IsSender = false;
        }

        private void ChangeVisible(int control)
        {
            if (searchBorder.IsVisible && !(control == 1))
            {
                searchBorder.Visibility = Visibility.Hidden;
                search.Text = "";
            }

            if (profileMenu.IsVisible && !(control == 2))
            {
                profileMenu.Visibility = Visibility.Hidden;
            }

            if (catalog.IsVisible && !(control == 3))
            {
                catalog.Visibility = Visibility.Hidden;
            }

            if (editComic.IsVisible && !(control == 4))
            {
                editComic.Visibility = Visibility.Hidden;
            }
        }


        private void ProfileIconClick(object sender, RoutedEventArgs e)
        {
            if (profileMenu.IsVisible)
                profileMenu.Visibility = Visibility.Hidden;
            else
                profileMenu.Visibility = Visibility.Visible;
            editComic.Visibility = Visibility.Hidden;

            ChangeVisible(2);
        }


        #region Profile Menu Button Clicks
        private void ProfileClick(object sender, RoutedEventArgs e)
        {
            profileMenu.Visibility = Visibility.Hidden;

            if (!(viewFrame.Content is ProfileEdit))
            {
                if (viewFrame.NavigationService.CanGoBack)
                {
                    viewFrame.NavigationService.RemoveBackEntry();
                }
                viewFrame.NavigationService.Navigate(new ProfileEdit());
            }
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

            ChangeVisible(1);
        }

        private void ClearSearchLinkClick(object sender, RoutedEventArgs e)
        {
            search.Text = "";
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

            ChangeVisible(3);
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
            else if (catalog.SelectedIndex == 2)
            {
                if (viewFrame.Content is ProfileEdit)
                {
                    (viewFrame.Content as ProfileEdit).email.Text = "swdwdwd";
                }
            }
        }

        #endregion

        private void CreateComicClick(object sender, RoutedEventArgs e)
        {
            if (editComic.IsVisible)
                editComic.Visibility = Visibility.Hidden;
            else
                editComic.Visibility = Visibility.Visible;

            profileMenu.Visibility = Visibility.Hidden;

            ChangeVisible(4);
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            ChangeVisible(0);

            if (!(viewFrame.Content is ComicListPage))
            {
                if (viewFrame.NavigationService.CanGoBack)
                {
                    viewFrame.NavigationService.RemoveBackEntry();
                }
                viewFrame.NavigationService.Navigate(new ComicListPage());
            }
        }

        private void EditComicSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editComic.SelectedIndex == 0)
            {
                if (!(viewFrame.Content is ComicCreatePage))
                {
                    if (viewFrame.NavigationService.CanGoBack)
                    {
                        viewFrame.NavigationService.RemoveBackEntry();
                    }
                    viewFrame.NavigationService.Navigate(new ComicCreatePage(null));
                }
                editComic.SelectedItem = null;
            }
            editComic.Visibility = Visibility.Hidden;
        }
    }
}
