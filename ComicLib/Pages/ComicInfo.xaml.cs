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
    /// Логика взаимодействия для ComicInfo.xaml
    /// </summary>
    public partial class ComicInfo : System.Windows.Controls.Page
    {
        private Comic Comic { get; set; }
        private bool Check { get; set; } = true;
        private bool IsMyFavourite { get; set; }
        public ComicInfo(Comic comic, bool i)
        {
            InitializeComponent();

            IsMyFavourite = i;
            Comic = comic;
            DataContext = Comic;

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            comicInfoFrame.Navigate(new ComicCover(comic));
            infoTB.IsChecked = true;

            if (DBHelper.CurrentUser.UserAndFavorite.Where(uac => uac.Id_Comic == comic.Id_Comic).FirstOrDefault() != null)
            {
                Check = false;
                realBookmark.IsChecked = true;
            }

            if (comic.ComicAndAuthor.Count == 2)
            {
                authorTagA.Visibility = Visibility.Visible;
                authorTagB.Visibility = Visibility.Visible;

                authorTagA.DataContext = DBHelper.Authors.Where(a => a.Id_Author == comic.ComicAndAuthor.FirstOrDefault().Author.Id_Author).FirstOrDefault();
                authorTagB.DataContext = DBHelper.Authors.Where(a => a.Id_Author == comic.ComicAndAuthor.LastOrDefault().Author.Id_Author).FirstOrDefault();

                author1.Text = comic.ComicAndAuthor.FirstOrDefault().Author.Name;
                author2.Text = comic.ComicAndAuthor.LastOrDefault().Author.Name;
            }
            else
            {
                authorTagA.Visibility = Visibility.Visible;

                authorTagA.DataContext = DBHelper.Authors.Where(a => a.Id_Author == comic.ComicAndAuthor.FirstOrDefault().Author.Id_Author).FirstOrDefault();

                author1.Text = comic.ComicAndAuthor.FirstOrDefault().Author.Name;
            }
        }

        private void GetBackClick(object sender, RoutedEventArgs e)
        {
            if (!IsMyFavourite)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
                NavigationService.Navigate(new ComicListPage());
            }
            else
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
                NavigationService.Navigate(new MyComicAdds(false));
            }
        }

        private void RealBookmarkChecked(object sender, RoutedEventArgs e)
        {
            if (Check)
            {
                DBHelper.DBContext.UserAndFavorite.Add(new UserAndFavorite
                {
                    Id_User = DBHelper.CurrentUser.Id_User,
                    Id_Comic = Comic.Id_Comic
                });

                DBHelper.DBContext.SaveChanges();
            }
            Check = true;
        }

        private void RealBookmarkUnchecked(object sender, RoutedEventArgs e)
        {
            UserAndFavorite userAndFavorite = DBHelper.DBContext.UserAndFavorite.Where(uac => uac.Id_Comic == Comic.Id_Comic && uac.Id_User == DBHelper.CurrentUser.Id_User).FirstOrDefault();
            if (userAndFavorite != null)
            {
                DBHelper.DBContext.UserAndFavorite.Remove(userAndFavorite);
                DBHelper.DBContext.SaveChanges();
            }
        }

        private void Unchecker(int choice)
        {
            if (choice != 1)
            {
                infoTB.IsChecked = false;
            }
            if (choice != 2)
            {
                commentsTB.IsChecked = false;
            }
        }

        private void InfoTBChecked(object sender, RoutedEventArgs e)
        {
            Unchecker(1);
            infoTB.IsEnabled = false;

            if (comicInfoFrame.NavigationService.CanGoBack)
            {
                comicInfoFrame.NavigationService.RemoveBackEntry();
            }
            if (!(comicInfoFrame.Content is ComicCover))
            {
                if (comicInfoFrame.Content is ComicComments)
                {
                    (comicInfoFrame.Content as ComicComments).mainGrid.Children.Clear();
                }
                comicInfoFrame.Navigate(new ComicCover(Comic));
            }
        }
        private void InfoTBUnchecked(object sender, RoutedEventArgs e)
        {
            infoTB.IsEnabled = true;
        }

        private void CommentsTBChecked(object sender, RoutedEventArgs e)
        {
            Unchecker(2);
            commentsTB.IsEnabled = false;

            if (comicInfoFrame.NavigationService.CanGoBack)
            {
                comicInfoFrame.NavigationService.RemoveBackEntry();
            }
            if (!(comicInfoFrame.Content is ComicComments))
            {
                if (comicInfoFrame.Content is ComicCover)
                {
                    (comicInfoFrame.Content as ComicCover).mainGrid.Children.Clear();
                }
                comicInfoFrame.Navigate(new ComicComments(Comic));
            }
        }
        private void CommentsTBUnchecked(object sender, RoutedEventArgs e)
        {
            commentsTB.IsEnabled = true;
        }

        private void ComicInfoFrameContentRendered(object sender, EventArgs e)
        {
            if (comicInfoFrame.Content is ComicCover)
            {
                infoTB.IsChecked = true;
                Unchecker(1);
            }
            else if (comicInfoFrame.Content is ComicComments)
            {
                commentsTB.IsChecked = true;
                Unchecker(2);
            }
        }

        private void StartReadingClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new ReadComicPages(Comic));
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            cover.Source = null;
            mainGrid.Children.Clear();
        }
    }
}
