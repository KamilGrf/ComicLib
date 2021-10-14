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
            //if (DBHelper.CurrentUser.Avatar != null)
            //{
            //    using (MemoryStream byteStream = new MemoryStream(DBHelper.CurrentUser.Avatar))
            //    {
            //        BitmapImage image = new BitmapImage();
            //        image.BeginInit();
            //        image.CacheOption = BitmapCacheOption.OnLoad;
            //        image.StreamSource = byteStream;
            //        image.EndInit();
            //        ava.Source = image;
            //    }
            //}
            if (DBHelper.CurrentUser.Rank == 2)
            {
                rank.IsChecked = true;
            }
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                if (Path.EndsWith(".png") || Path.EndsWith(".jpg") || Path.EndsWith(".jpeg"))
                {
                    ImageConverter converter = new ImageConverter();
                    BitmapImage image = converter.BitmapImageConvert(Path);

                    //ava.Source = new BitmapImage(new Uri(path));
                    ava.Source = image;
                }
            }
            //(string[])e.Data.GetData(DataFormats.FileDrop)
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
                {
                    user.Email = email.Text;
                }
                if (ava.Source != null)
                {
                    user.Avatar = File.ReadAllBytes(Path);
                }
                user.Name = nick.Text;
                user.Password = pas.Text;
                user.Rank = Rank;
                
                DBHelper.DBContext.SaveChanges();

                DBHelper.CurrentUser = user;

                ImageConverter converter = new ImageConverter();
                BitmapImage image = converter.BitmapImageConvert(Path);

                DBHelper.Image = image;
                (OtherHelper.MainWindow.mainFrame.Content as ViewPort).profileIcon.Source = image;

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
    }
}
