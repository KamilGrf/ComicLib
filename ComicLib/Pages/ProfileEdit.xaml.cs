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
using System.Runtime.ConstrainedExecution;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileEdit.xaml
    /// </summary>
    public partial class ProfileEdit : System.Windows.Controls.Page
    {
        public bool Rank 
        { 
            get 
            {
                if (rank.IsChecked.Value)
                    return true;
                else
                    return false;
            } 
        }

        public string Path { get; set; }

        public ProfileEdit()
        {
            DataContext = DBHelper.CurrentUser;

            InitializeComponent();

            nick.Text = DBHelper.CurrentUser.Name;
            email.Text = DBHelper.CurrentUser.Email;
            pas.Text = DBHelper.CurrentUser.Password;

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            if (DBHelper.CurrentUser.Avatar == null)
            {
                deleteButton.Visibility = Visibility.Hidden;
            }

            if (DBHelper.CurrentUser.Rank == true)
            {
                rank.IsChecked = true;
            }
        }

        private void ImageDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string fileName = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                Path = fileName;

                if (Path.EndsWith(".png") || Path.EndsWith(".jpg") || Path.EndsWith(".jpeg"))
                {
                    ImageConverter converter = new ImageConverter();
                    ava.Source = converter.BitmapImageConvert(Path);
                    deleteButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ComicMessageBox comicMessageBox = new ComicMessageBox("Внимание!", "Изображение должно быть только форматов: png, jpg, jpeg")
                    {
                        Owner = OtherHelper.MainWindow
                    };
                    comicMessageBox.ShowDialog();
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
            if (nick.Text.Length == 0 || email.Text.Length == 0 || pas.Text.Length == 0)
            {
                ComicMessageBox comicMessageBox = new ComicMessageBox("Внимание!", "Поля должны быть заполнены!")
                {
                    Owner = OtherHelper.MainWindow
                };
                comicMessageBox.ShowDialog();
            }
            else
            {
                ImageConverter converter = new ImageConverter();
                User mainUser = this.DataContext as User;
                User user = DBHelper.DBContext.User.Where(u => u.Id_User == mainUser.Id_User).FirstOrDefault();

                if (email.Text != null)
                    user.Email = email.Text;

                if (ava.Source != null)
                {
                    if (Path != null)
                    {
                        using (MemoryStream resizer = new ImageResizing(Path).Resize(200, 200).Quality(40).ToStream())
                        {
                            var imageBytes = resizer.ToArray();
                            user.Avatar = imageBytes;

                            DBHelper.Image = converter.BitmapImageFromByteArrayConvert(imageBytes);
                            (OtherHelper.MainWindow.mainFrame.Content as ViewPort).profileIcon.Source = converter.BitmapImageFromByteArrayConvert(imageBytes);
                        }
                    }
                }
                else
                {
                    user.Avatar = null; 
                    (OtherHelper.MainWindow.mainFrame.Content as ViewPort).profileIcon.Source = null;
                }

                user.Name = nick.Text;
                user.Password = pas.Text;
                user.Rank = Rank;


                if (Rank)
                    (OtherHelper.MainWindow.mainFrame.Content as ViewPort).comicAdd.Visibility = Visibility.Visible;
                else
                    (OtherHelper.MainWindow.mainFrame.Content as ViewPort).comicAdd.Visibility = Visibility.Hidden;


                DBHelper.DBContext.SaveChanges();
                DBHelper.CurrentUser = user;

                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                    NavigationService.RemoveBackEntry();
                }
            }
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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            mainBorder.Child = null;
        }
    }
}
