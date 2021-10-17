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
using System.IO;
using System.Windows.Media.Effects;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComicCreatePage.xaml
    /// </summary>
    public partial class ComicCreatePage : System.Windows.Controls.Page
    {
        private TextBlock NoElementText { get; set; } = new TextBlock
        {
            Text = "Нет элементов",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 15
        };

        /// <summary>
        /// Путь к изображению
        /// </summary>
        public string Path { get; set; }

        public Comic CurrentComic { get; set; }

        public ComicCreatePage(Comic comic)
        {
            InitializeComponent();

            CurrentComic = comic;

            NoElementText.Visibility = Visibility.Hidden;
            elementsPanel.Height = 0;

            if (comic == null)
            {
                deleteButton.Visibility = Visibility.Hidden;
            }

            AuthorsListBoxCreate(DBHelper.Authors);
        }

        #region Authors List Create
        /// <summary>
        /// Создание визуального элемента в виде листа авторов комиксов
        /// </summary>
        /// <param name="authors">Лист авторов для вывода</param>
        public void AuthorsListBoxCreate(List<Author> authors)
        {
            Border border = new Border()
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(60, 0, 60, 0),
                Height = 158,
                Background = Brushes.White,
                BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179)),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(4),
                Effect = new DropShadowEffect
                {
                    ShadowDepth = 0.5,
                    Opacity = 0.4,
                    BlurRadius = 10
                }
            };
            listGrid.Children.Add(border);
            listGrid.Children.Add(NoElementText);

            ScrollViewer scrollViewer = new ScrollViewer
            {
                Padding = new Thickness(10)
            };
            border.Child = scrollViewer;

            StackPanel stackPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };
            scrollViewer.Content = stackPanel;

            foreach (var author in authors)
            {
                Border authorBorder = new Border
                {
                    Background = Brushes.Transparent,
                    CornerRadius = new CornerRadius(4),
                    BorderThickness = new Thickness(2),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179)),
                    Height = 40,
                    Margin = new Thickness(2)
                };
                stackPanel.Children.Add(authorBorder);

                authorBorder.MouseEnter += (sender, e) =>
                {
                    Cursor = Cursors.Hand;
                    authorBorder.Background = Brushes.LightGray;
                };
                authorBorder.MouseLeave += (sender, e) =>
                {
                    Cursor = Cursors.Arrow;
                    authorBorder.Background = Brushes.Transparent;
                };

                TextBlock textBlock = new TextBlock
                {
                    Text = author.Name,
                    Margin = new Thickness(10, 0, 10, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                authorBorder.Child = textBlock;

                authorBorder.MouseDown += (sender, e) =>
                {
                    if (authorA.Text != "")
                    {
                        if (authorA.Text != author.Name)
                        {
                            authorB.Text = author.Name;

                            authorTagB.Visibility = Visibility.Visible;

                            authorFind.IsEnabled = false;
                            authorFind.Text = "";
                        }
                    }
                    else
                    {
                        authorA.Text = author.Name;

                        authorTagA.Visibility = Visibility.Visible;
                        authorFind.Text = "";

                        elementsPanel.Visibility = Visibility.Visible;
                        elementsPanel.Height = double.NaN;
                    }
                };
            }
        }
        #endregion

        #region Работа с изображением
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
        #endregion

        
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
            listGrid.Children.Clear();
            if (authorFind.Text == "")
            {
                AuthorsListBoxCreate(DBHelper.Authors);
                NoElementText.Visibility = Visibility.Hidden;
            }
            else
            {
                List<Author> authors = DBHelper.Authors.Where(a => a.Name.ToLower().Contains(authorFind.Text.ToLower())).ToList();
                AuthorsListBoxCreate(authors);

                if (authors.Count == 0)
                {
                    NoElementText.Visibility = Visibility.Visible;
                }
                else
                {
                    NoElementText.Visibility = Visibility.Hidden;
                }
            }
        }
        private void AuthorARemoveClick(object sender, RoutedEventArgs e)
        {
            if (authorB.Text != "")
            {
                authorA.Text = authorB.Text;

                authorB.Text = "";
                authorTagB.Visibility = Visibility.Hidden;

                authorFind.IsEnabled = true;
                authorFind.Text = "";
            }
            else
            {
                authorTagA.Visibility = Visibility.Hidden;

                elementsPanel.Visibility = Visibility.Hidden;
                elementsPanel.Height = 0;

                authorA.Text = "";
                authorFind.Text = "";
            }
        }
        private void AuthorBRemoveClick(object sender, RoutedEventArgs e)
        {
            authorTagB.Visibility = Visibility.Hidden;
            authorB.Text = "";

            authorFind.Text = "";
            authorFind.IsEnabled = true;
        }
        private void AuthorAddFrameContentRendered(object sender, EventArgs e)
        {
            if (authorAddFrame.Content != null)
            {
                mainGrid.Height = 900;
                splitter.Height = mainBorder.ActualHeight - mainGrid.Margin.Top * 2 - 60;
                splitter.VerticalAlignment = VerticalAlignment.Top;
            }
            else
            {
                mainGrid.Height = double.NaN;
                splitter.Height = double.NaN;
                splitter.VerticalAlignment = VerticalAlignment.Stretch;
            }
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (authorAddFrame.Content != null)
            {
                mainGrid.Height = 900;
                splitter.Height = mainBorder.ActualHeight - mainGrid.Margin.Top * 2 - 60;
                splitter.VerticalAlignment = VerticalAlignment.Top;
            }
            else
            {
                mainGrid.Height = double.NaN;
                splitter.Height = double.NaN;
                splitter.VerticalAlignment = VerticalAlignment.Stretch;
            }
        }


        private void ComicAddClick(object sender, RoutedEventArgs e)
        {
            if (title.Text == "" || year.Text == "" || info.Text == "" || Path == "" || authorA.Text == "")
            {
                MessageBox.Show("Заполните пустые поля");
            }
            else
            {
                Comic comic = new Comic
                {
                    Title = title.Text,
                    Year = (short)Convert.ToInt32(year.Text),
                    Info = info.Text,
                    Cover = File.ReadAllBytes(Path),
                    Id_User = DBHelper.CurrentUser.Id_User
                };

                DBHelper.DBContext.Comic.Add(comic);
                DBHelper.DBContext.SaveChanges();

                DBHelper.CurrentUser.Comic.Add(comic);

                DBHelper.Comics = DBHelper.DBContext.Comic.ToList();

                if (authorB.Text != "")
                {
                    List<ComicAndAuthor> comicAndAuthors = new List<ComicAndAuthor>
                    {
                        new ComicAndAuthor
                        {
                            Id_Author = DBHelper.Authors.Where(a => a.Name == authorA.Text).FirstOrDefault().Id_Author,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        },
                        new ComicAndAuthor
                        {
                            Id_Author = DBHelper.Authors.Where(a => a.Name == authorB.Text).FirstOrDefault().Id_Author,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        }
                    };

                    DBHelper.DBContext.ComicAndAuthor.AddRange(comicAndAuthors);
                    DBHelper.DBContext.SaveChanges();
                }
                else
                {
                    ComicAndAuthor comicAndAuthorFirst = new ComicAndAuthor
                    {
                        Id_Author = DBHelper.Authors.Where(a => a.Name == authorA.Text).FirstOrDefault().Id_Author,
                        Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                    };

                    DBHelper.DBContext.ComicAndAuthor.Add(comicAndAuthorFirst);
                    DBHelper.DBContext.SaveChanges();
                }

                GetBackClick(sender, e);
            }
        }
    }
}
