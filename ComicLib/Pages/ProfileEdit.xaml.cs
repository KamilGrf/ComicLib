using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
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
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileEdit.xaml
    /// </summary>
    public partial class ProfileEdit : System.Windows.Controls.Page
    {
        public decimal Rank 
        { 
            get 
            {
                if (rank.IsChecked.Value)
                    return 2;
                else
                    return 1;
            } 
        }

        public string Path { get; set; }

        public ProfileEdit()
        {
            InitializeComponent();

            DataContext = DBHelper.CurrentUser;

            if (DBHelper.CurrentUser.Avatar == null)
            {
                deleteButton.Visibility = Visibility.Hidden;
            }

            if (DBHelper.CurrentUser.Rank == 2)
            {
                rank.IsChecked = true;
            }
        }

        private void ImageDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                if (Path.EndsWith(".png") || Path.EndsWith(".jpg") || Path.EndsWith(".jpeg"))
                {
                    ImageConverter converter = new ImageConverter();
                    ava.Source = converter.BitmapImageConvert(Path);
                    deleteButton.Visibility = Visibility.Visible;
                }
            }
        }

        private void ImageOpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                Path = openFileDialog.FileName;
                ImageConverter converter = new ImageConverter();
                ava.Source = converter.BitmapImageConvert(Path);
                deleteButton.Visibility = Visibility.Visible;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (nick.Text == "" || email.Text == "" || pas.Text == "")
            {
                
            }
            else
            {
                User mainUser = this.DataContext as User;
                User user = DBHelper.DBContext.User.Where(u => u.Id_User == mainUser.Id_User).FirstOrDefault();

                if (email.Text != null)
                    user.Email = email.Text;

                if (Path != null)
                    user.Avatar = File.ReadAllBytes(Path);
                else
                    user.Avatar = null;

                user.Name = nick.Text;
                user.Password = pas.Text;
                user.Rank = Rank;
                
                DBHelper.DBContext.SaveChanges();

                DBHelper.CurrentUser = user;

                if (Path != null)
                {
                    ImageConverter converter = new ImageConverter();
                    BitmapImage image = converter.BitmapImageConvert(Path);

                    DBHelper.Image = image;
                    (OtherHelper.MainWindow.mainFrame.Content as ViewPort).profileIcon.Source = image;
                }
                else
                {
                    (OtherHelper.MainWindow.mainFrame.Content as ViewPort).profileIcon.Source = null;
                }
                

                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                    NavigationService.RemoveBackEntry();
                }
            }
        }

        private void RankChecked(object sender, RoutedEventArgs e)
        {

        }

        private void GetBackClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new ComicListPage());
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (ava.Source != null)
            {
                ava.Source = null;
                Path = null;
                deleteButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
