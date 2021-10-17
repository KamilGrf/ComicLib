using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AuthorAdd.xaml
    /// </summary>
    public partial class AuthorAdd : System.Windows.Controls.Page
    {
        public string Path { get; set; }
        public AuthorAdd()
        {
            InitializeComponent();
        }

        private void ImageDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                if (Path.EndsWith(".png") || Path.EndsWith(".jpg") || Path.EndsWith(".jpeg"))
                {
                    ImageConverter converter = new ImageConverter();
                    photo.Source = converter.BitmapImageConvert(Path);
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
                photo.Source = converter.BitmapImageConvert(Path);
                deleteButton.Visibility = Visibility.Visible;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Author author = new Author();

            if (Path != null)
                author.Photo = File.ReadAllBytes(Path);
            else
                author.Photo = null;

            author.Name = name.Text;
            author.Info = info.Text;

            DBHelper.DBContext.Author.Add(author);
            DBHelper.DBContext.SaveChanges();
            DBHelper.Authors = DBHelper.DBContext.Author.ToList();

            ((OtherHelper.MainWindow.mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).listGrid.Children.Clear();
            ((OtherHelper.MainWindow.mainFrame.Content as ViewPort).viewFrame.Content as ComicCreatePage).AuthorsListBoxCreate(DBHelper.Authors);
            NavigationService.Content = null;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Content = null;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (photo.Source != null)
            {
                photo.Source = null;
                Path = null;
                deleteButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
