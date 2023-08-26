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

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                    NavigationService.RemoveBackEntry();
            }

            if (DBHelper.CurrentUser.Rank == false)
                comicAdd.Visibility = Visibility.Hidden;
            else
                comicAdd.Visibility = Visibility.Visible;


            DataContext = DBHelper.CurrentUser;

            profileMenu.Visibility = Visibility.Hidden;
            editComic.Visibility = Visibility.Hidden;

            numOfComics.Text = DBHelper.CurrentUser.Comic.Count.ToString();
        }

        private void MainGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox)
                IsSender = true;
            
            if (IsSender == false)
                ChangeVisible(0);

            if (sender is Border)
                IsSender = false;
        }

        public void ChangeVisible(int control)
        {
            if (profileMenu.IsVisible && !(control == 2))
                profileMenu.Visibility = Visibility.Hidden;

            if (editComic.IsVisible && !(control == 4))
                editComic.Visibility = Visibility.Hidden;
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
                    viewFrame.NavigationService.RemoveBackEntry();

                viewFrame.NavigationService.Navigate(new ProfileEdit());
            }
        }

        private void MineBookmarksClick(object sender, RoutedEventArgs e)
        {
            profileMenu.Visibility = Visibility.Hidden;

            if (!(viewFrame.Content is MyComicAdds))
            {
                if (viewFrame.NavigationService.CanGoBack)
                    viewFrame.NavigationService.RemoveBackEntry();

                viewFrame.NavigationService.Navigate(new MyComicAdds(false));
            }
        }

        private void AccLeaveClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();

            NavigationService.Navigate(new LoginPage());
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
                    viewFrame.NavigationService.RemoveBackEntry();

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
                        viewFrame.NavigationService.RemoveBackEntry();

                    viewFrame.NavigationService.Navigate(new ComicCreatePage(null));
                }
                editComic.SelectedItem = null;
            }
            else if (editComic.SelectedIndex == 1)
            {
                if (!(viewFrame.Content is MyComicAdds))
                {
                    if (viewFrame.NavigationService.CanGoBack)
                        viewFrame.NavigationService.RemoveBackEntry();

                    viewFrame.NavigationService.Navigate(new MyComicAdds(true));
                }
                editComic.SelectedItem = null;
            }
            editComic.Visibility = Visibility.Hidden;
        }
    }
}
