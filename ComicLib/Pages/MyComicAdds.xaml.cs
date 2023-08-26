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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComicLib.ClassHelpers;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для MyComicAdds.xaml
    /// </summary>
    public partial class MyComicAdds : System.Windows.Controls.Page
    {
        private bool IsMyAdds { get; set; }
        public MyComicAdds(bool i)
        {
            InitializeComponent();

            IsMyAdds = i;

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            if (i)
            {
                editComic.ItemsSource = DBHelper.CurrentUser.Comic.ToList();
                if (editComic.Items.IsEmpty)
                {
                    noElements.Visibility = Visibility.Visible;
                }
            }
            else
            {
                header.Text = "Избранные комиксы";
                favouriteComic.ItemsSource = DBHelper.CurrentUser.UserAndFavorite.ToList();
                if (favouriteComic.Items.IsEmpty)
                {
                    noElements.Visibility = Visibility.Visible;
                }
                editComic.Visibility = Visibility.Hidden;
                favouriteComic.Visibility = Visibility.Visible;
            }
        }

        private void EditComicSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editComic.SelectedItem != null)
            {
                NavigationService.Navigate(new ComicEditPage(editComic.SelectedItem as Comic));
                editComic.SelectedItem = null;
            }
        }

        private void FavouriteComicSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (favouriteComic.SelectedItem != null)
            {
                NavigationService.Navigate(new ComicInfo((favouriteComic.SelectedItem as UserAndFavorite).Comic, true));
                favouriteComic.SelectedItem = null;
            }
        }

        private void ComicFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsMyAdds)
            {
                if (comicFilter.Text != "")
                {
                    filterBack.Visibility = Visibility.Hidden;

                    editComic.ItemsSource = DBHelper.CurrentUser.Comic.Where(c => c.Title.ToLower().Contains(comicFilter.Text.ToLower())).ToList();
                    if (editComic.Items.IsEmpty)
                    {
                        noElements.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        noElements.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    editComic.ItemsSource = DBHelper.CurrentUser.Comic.ToList();
                    if (editComic.Items.IsEmpty)
                    {
                        noElements.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        noElements.Visibility = Visibility.Hidden;
                    }
                    filterBack.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (comicFilter.Text != "")
                {
                    filterBack.Visibility = Visibility.Hidden;

                    favouriteComic.ItemsSource = DBHelper.CurrentUser.UserAndFavorite.Where(c => c.Comic.Title.ToLower().Contains(comicFilter.Text.ToLower())).ToList();
                    if (favouriteComic.Items.IsEmpty)
                    {
                        noElements.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        noElements.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    favouriteComic.ItemsSource = DBHelper.CurrentUser.UserAndFavorite.ToList();
                    if (favouriteComic.Items.IsEmpty)
                    {
                        noElements.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        noElements.Visibility = Visibility.Hidden;
                    }
                    filterBack.Visibility = Visibility.Visible;
                }
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new ComicListPage());
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            favouriteComic.ItemsSource = null;
            editComic.ItemsSource = null;
            mainGrid.Children.Clear();
        }
    }
}
