using Microsoft.Win32;
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
    /// Логика взаимодействия для ComicCreatePage.xaml
    /// </summary>
    public partial class ComicCreatePage : System.Windows.Controls.Page
    {
        public string Path { get; set; }
        public Comic CurrentComic { get; set; }
        public ComicCreatePage(Comic comic)
        {
            InitializeComponent();

            authorList.ItemsSource = DBHelper.Authors;
            CurrentComic = comic;

            if (comic == null)
            {
                deleteButton.Visibility = Visibility.Hidden;
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
                    cover.Source = converter.BitmapImageConvert(Path);
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
                cover.Source = converter.BitmapImageConvert(Path);
                deleteButton.Visibility = Visibility.Visible;
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (cover.Source != null)
            {
                cover.Source = null;
                Path = null;
                deleteButton.Visibility = Visibility.Hidden;
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

        private void AuthorAddClick(object sender, RoutedEventArgs e)
        {
            authorAddFrame.Content = new AuthorAdd();
        }

        private void AuthorFindTextChanged(object sender, TextChangedEventArgs e)
        {
            authorList.UnselectAll();
            if (authorFind.Text == "")
            {
                authorList.ItemsSource = DBHelper.Authors;
            }
            else
            {
                authorList.ItemsSource = DBHelper.Authors.Where(a => a.Name.ToLower().Contains(authorFind.Text.ToLower()));
            }
        }

        private void AuthorListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(authorList.SelectedItem is Author authorObject))
            {

            }
            else if (authorA.Text != "")
            {
                if (authorA.Text != authorObject.Name)
                {
                    authorB.Text = authorObject.Name;
                    authorTagB.Visibility = Visibility.Visible;
                    authorFind.IsEnabled = false;
                    authorFind.Text = "";
                }
            }
            else
            {
                authorA.Text = authorObject.Name;
                authorTagA.Visibility = Visibility.Visible;
            }
        }

        private void AuthorARemoveClick(object sender, RoutedEventArgs e)
        {
            if (authorB.Text != "")
            {
                authorA.Text = authorB.Text;
                authorTagB.Visibility = Visibility.Hidden;
                authorFind.IsEnabled = true;
            }
            else
            {
                authorTagA.Visibility = Visibility.Hidden;
                authorA.Text = "";
            }
        }

        private void AuthorBRemoveClick(object sender, RoutedEventArgs e)
        {
            authorTagB.Visibility = Visibility.Hidden;
            authorB.Text = "";
            authorFind.IsEnabled = true;
        }
    }
}
