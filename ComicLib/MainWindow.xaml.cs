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
using ComicLib.Pages;
using ComicLib.ClassHelpers;
using ComicLib.Properties;

namespace ComicLib
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DBHelper.Authors = DBHelper.DBContext.Author.OrderBy(a => a.Name).ToList();
            DBHelper.Comics = DBHelper.DBContext.Comic.OrderBy(a => a.Title).ToList();
            DBHelper.Genres = DBHelper.DBContext.Genre.OrderBy(a => a.Title).ToList();
            DBHelper.Tags = DBHelper.DBContext.Tag.OrderBy(a => a.Title).ToList();

            if (Settings.Default.Password != ""
                && Settings.Default.LastLoginDate.AddMonths(1) > DateTime.Now)
            {
                DBHelper.CurrentUser = DBHelper.DBContext.User
                    .Where(u => u.Name == Settings.Default.Name
                    && u.Password == Settings.Default.Password).FirstOrDefault();
                mainFrame.Content = new ViewPort();
            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void MenuClick(object sender, RoutedEventArgs e)
        {
            Point pointToWindow = Mouse.GetPosition(this);
            Point pointToScreen = PointToScreen(pointToWindow);

            SystemCommands.ShowSystemMenu(this, pointToScreen);
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mainFrame.Content is ViewPort)
            {
                if ((mainFrame.Content as ViewPort).viewFrame.Content is ComicInfo)
                {
                    if (((mainFrame.Content as ViewPort).viewFrame.Content as ComicInfo).comicInfoFrame.Content is ComicComments)
                    {
                        (((mainFrame.Content as ViewPort).viewFrame.Content as ComicInfo).comicInfoFrame.Content as ComicComments).sortBack.Visibility = Visibility.Hidden;
                    }
                }
                else if ((mainFrame.Content as ViewPort).viewFrame.Content is ComicCreatePage)
                {
                    ((mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).listGrid.Visibility = Visibility.Hidden;
                    ((mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).listGrid.Height = 0;

                    ((mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).listGenre.Visibility = Visibility.Hidden;
                    ((mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).listGenre.Height = 0;

                    ((mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).listTags.Visibility = Visibility.Hidden;
                    ((mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).listTags.Height = 0;

                    ((mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).mainGrid.Height = double.NaN;
                    ((mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).splitter.Height = double.NaN;
                    ((mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).splitter.VerticalAlignment = VerticalAlignment.Stretch;
                }
            }
            clearFocus.Focus();
        }
    }
}
