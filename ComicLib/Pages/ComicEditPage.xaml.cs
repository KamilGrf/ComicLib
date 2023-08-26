using ComicLib.ClassHelpers;
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

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComicEditPage.xaml
    /// </summary>
    public partial class ComicEditPage : System.Windows.Controls.Page
    {
        private Comic Comic { get; set; }
        public ComicEditPage(Comic comic)
        {
            InitializeComponent();

            Comic = comic;
            DataContext = comic;

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
        }

        private void PageMouseDown(object sender, MouseButtonEventArgs e)
        {
            (OtherHelper.MainWindow.mainFrame.Content as ViewPort).ChangeVisible(0);
        }

        private void PagesAddClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new ComicPagesAdd(Comic));
        }

        private void ComicEditClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new ComicCreatePage(Comic));
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new MyComicAdds(true));
        }

        private void ComicDeleteButton(object sender, RoutedEventArgs e)
        {
            ComicMessageBox comicMessageBox = new ComicMessageBox("Внимание!", "Вы точно хотите удалить комикс?", true)
            {
                Owner = OtherHelper.MainWindow
            };
            comicMessageBox.ShowDialog();

            if (OtherHelper.OkOrNot)
            {
                DBHelper.DBContext.ComicAndAuthor.RemoveRange(Comic.ComicAndAuthor);
                DBHelper.DBContext.ComicAndGenre.RemoveRange(Comic.ComicAndGenre);
                DBHelper.DBContext.ComicAndTag.RemoveRange(Comic.ComicAndTag);
                DBHelper.DBContext.Comment.RemoveRange(Comic.Comment);
                DBHelper.DBContext.UserAndFavorite.RemoveRange(Comic.UserAndFavorite);
                DBHelper.DBContext.Page.RemoveRange(Comic.Page);

                DBHelper.DBContext.Comic.Remove(Comic);
                DBHelper.DBContext.SaveChanges();

                (OtherHelper.MainWindow.mainFrame.Content as ViewPort).numOfComics.Text = DBHelper.CurrentUser.Comic.Count.ToString();

                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
                NavigationService.Navigate(new ComicListPage());
            }
        }
    }
}
