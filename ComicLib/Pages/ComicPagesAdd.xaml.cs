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
using ComicLib;
using ComicLib.ClassHelpers;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComicPagesAdd.xaml
    /// </summary>
    public partial class ComicPagesAdd : System.Windows.Controls.Page
    {
        private List<PageImage> Images { get; set; } = new List<PageImage>();
        private int Number { get; set; }
        private Comic Comic { get; set; }
        public ComicPagesAdd(Comic comic)
        {
            InitializeComponent();

            Comic = comic;
            DataContext = Comic;

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            Images.Add(null);

            if (Comic.Page.Count != 0)
            {
                foreach (var page in Comic.Page.ToList())
                {
                    Images.Add(new PageImage
                    {
                        Number = (int)page.Number,
                        Photo = page.Photo
                    });
                }
                Number = Comic.Page.Count;
                pagesList.ItemsSource = Images;
            }
        }
        private void PageMouseDown(object sender, MouseButtonEventArgs e)
        {
            (OtherHelper.MainWindow.mainFrame.Content as ViewPort).ChangeVisible(0);
        }


        private void GoBackClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new ComicEditPage(Comic));
        }

        #region Image Work
        private void ImageAddDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                if (path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
                {
                    Number += 1;

                    using (var resizer = new ImageResizing(path).Resize(900, 1400).Quality(80).ToStream())
                    {
                        var imageBytes = resizer.ToArray();
                        Images.Add(new PageImage { Photo = imageBytes, Number = Number });
                    }

                    pagesList.ItemsSource = null;
                    pagesList.ItemsSource = Images;
                }
            }
        }

        private void ImageAddClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                Title = "Выберите изображение страницы"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;

                Number += 1;

                using (var resizer = new ImageResizing(path).Resize(900, 1400).Quality(80).ToStream())
                {
                    var imageBytes = resizer.ToArray();
                    Images.Add(new PageImage { Photo = imageBytes, Number = Number });
                }

                pagesList.ItemsSource = null;
                pagesList.ItemsSource = Images;
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            Images.Remove((PageImage)(sender as Button).DataContext);

            Number -= 1;

            for (int i = 1; i < Images.Count; i++)
            {
                Images[i].Number = i;
            }

            pagesList.ItemsSource = null;
            pagesList.ItemsSource = Images;
        }
        #endregion

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (Images.Count > 3 && Images.Count < 27)
            {
                if (Comic.Page.Count != 0)
                {
                    DBHelper.DBContext.Page.RemoveRange(Comic.Page);
                    DBHelper.DBContext.SaveChanges();
                }

                for (int i = 1; i < Images.Count; i++)
                {
                    Page page = new Page
                    {
                        Id_Comic = Comic.Id_Comic,
                        Photo = Images[i].Photo,
                        Number = (short)Images[i].Number
                    };

                    DBHelper.DBContext.Page.Add(page);
                }
                DBHelper.DBContext.SaveChanges();

                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
                NavigationService.Navigate(new ComicListPage());
            }
            else
            {
                ComicMessageBox comicMessageBox = new ComicMessageBox("Внимание!", "Должно быть добавлено от 3 до 25 страниц")
                {
                    Owner = OtherHelper.MainWindow
                };
                comicMessageBox.ShowDialog();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Images.Clear();
            pagesList.ItemsSource = null;
            mainGrid.Children.Clear();
        }
    }
}
