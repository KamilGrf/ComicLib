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
using System.Text.RegularExpressions;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComicCreatePage.xaml
    /// </summary>
    public partial class ComicCreatePage : System.Windows.Controls.Page
    {
        public TextBlock AuthorsNoElementText { get; set; } = new TextBlock
        {
            Text = "Нет элементов",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 15
        };

        private TextBlock GenresNoElementText { get; set; } = new TextBlock
        {
            Text = "Нет элементов",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 15
        };

        private TextBlock TagsNoElementText { get; set; } = new TextBlock
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

        private bool photoBool;
        private bool yearBool;
        private bool infoBool;
        private bool titleBool;
        private bool authorBool;
        private bool genreBool;
        private bool tagBool;
        public bool AllBool 
        { 
            get
            {
                if (photoBool && yearBool && infoBool && titleBool && authorBool && genreBool && tagBool)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ComicCreatePage(Comic comic)
        {
            InitializeComponent();

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            CurrentComic = comic;

            AuthorsNoElementText.Visibility = Visibility.Hidden;
            GenresNoElementText.Visibility = Visibility.Hidden;
            TagsNoElementText.Visibility = Visibility.Hidden;

            authorElementsPanel.Height = 0;
            genreElementsPanel.Height = 0;
            tagElementsPanel.Height = 0;

            if (comic == null)
            {
                deleteButton.Visibility = Visibility.Hidden;
            }

            if (CurrentComic != null)
            {
                DataContext = CurrentComic;

                if (CurrentComic.Cover != null)
                {
                    photoBool = true;
                }

                authorElementsPanel.Visibility = Visibility.Visible;
                authorElementsPanel.Height = double.NaN;
                authorBool = true;

                if (CurrentComic.ComicAndAuthor.Count == 2)
                {
                    authorA.Text = CurrentComic.ComicAndAuthor.FirstOrDefault().Author.Name;
                    authorTagA.Visibility = Visibility.Visible;

                    authorB.Text = CurrentComic.ComicAndAuthor.LastOrDefault().Author.Name;
                    authorTagB.Visibility = Visibility.Visible;
                }
                else
                {
                    authorA.Text = CurrentComic.ComicAndAuthor.FirstOrDefault().Author.Name;
                    authorTagA.Visibility = Visibility.Visible;
                }

                if (CurrentComic.ComicAndGenre.Count > 0)
                {
                    genreElementsPanel.Visibility = Visibility.Visible;
                    genreElementsPanel.Height = double.NaN;

                    genreBool = true;

                    int genreCount = CurrentComic.ComicAndGenre.Count;

                    if (genreCount == 1)
                    {
                        genreA.Text = CurrentComic.ComicAndGenre.FirstOrDefault().Genre.Title;
                        genreTagA.Visibility = Visibility.Visible;
                    }
                    else if (genreCount == 2)
                    {
                        genreA.Text = CurrentComic.ComicAndGenre.FirstOrDefault().Genre.Title;
                        genreTagA.Visibility = Visibility.Visible;

                        genreB.Text = CurrentComic.ComicAndGenre.LastOrDefault().Genre.Title;
                        genreTagB.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        genreA.Text = CurrentComic.ComicAndGenre.FirstOrDefault().Genre.Title;
                        genreTagA.Visibility = Visibility.Visible;

                        genreB.Text = CurrentComic.ComicAndGenre.Take(2).LastOrDefault().Genre.Title;
                        genreTagB.Visibility = Visibility.Visible;

                        genreC.Text = CurrentComic.ComicAndGenre.LastOrDefault().Genre.Title;
                        genreTagC.Visibility = Visibility.Visible;
                    }
                }

                if (CurrentComic.ComicAndTag.Count > 0)
                {
                    tagElementsPanel.Visibility = Visibility.Visible;
                    tagElementsPanel.Height = double.NaN;

                    tagBool = true;

                    int tagCount = CurrentComic.ComicAndTag.Count;

                    if (tagCount == 1)
                    {
                        tagA.Text = CurrentComic.ComicAndTag.FirstOrDefault().Tag.Title;
                        tagTagA.Visibility = Visibility.Visible;
                    }
                    else if (tagCount == 2)
                    {
                        tagA.Text = CurrentComic.ComicAndTag.FirstOrDefault().Tag.Title;
                        tagTagA.Visibility = Visibility.Visible;

                        tagB.Text = CurrentComic.ComicAndTag.LastOrDefault().Tag.Title;
                        tagTagB.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tagA.Text = CurrentComic.ComicAndTag.FirstOrDefault().Tag.Title;
                        tagTagA.Visibility = Visibility.Visible;

                        tagB.Text = CurrentComic.ComicAndTag.Take(2).LastOrDefault().Tag.Title;
                        tagTagB.Visibility = Visibility.Visible;

                        tagC.Text = CurrentComic.ComicAndTag.LastOrDefault().Tag.Title;
                        tagTagC.Visibility = Visibility.Visible;
                    }
                }
            }

            listGrid.Height = 0;
            listGenre.Height = 0;
            listTags.Height = 0;

            
            listGrid.Children.Add(ListCreate(DBHelper.Authors, null, null));
            listGrid.Children.Add(AuthorsNoElementText);

            listGenre.Children.Add(ListCreate(null, DBHelper.Genres, null));
            listGenre.Children.Add(GenresNoElementText);

            listTags.Children.Add(ListCreate(null, null, DBHelper.Tags));
            listTags.Children.Add(TagsNoElementText);
        }

        #region Authors List Create
        /// <summary>
        /// Создание визуального элемента в виде листа некоторой информации о комиксе
        /// </summary>
        /// <param name="authors">Лист авторов</param>
        /// <param name="genres">Лист жанров</param>
        /// <param name="tags">Лист тегов</param>
        /// <returns></returns>
        public Border ListCreate(List<Author> authors, List<Genre> genres, List<Tag> tags)
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

            if (authors != null)
            {
                foreach (var author in authors.OrderBy(a => a.Name))
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

                    authorBorder.MouseLeftButtonDown += (sender, e) =>
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

                            authorElementsPanel.Visibility = Visibility.Visible;
                            authorElementsPanel.Height = double.NaN;

                            authorBool = true;
                        }
                    };
                }
            }
            else if (genres != null)
            {
                foreach (var genre in genres.OrderBy(g => g.Title))
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
                        Text = genre.Title,
                        Margin = new Thickness(10, 0, 10, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    authorBorder.Child = textBlock;

                    authorBorder.MouseLeftButtonDown += (sender, e) =>
                    {
                        if (genreA.Text == "")
                        {
                            genreA.Text = genre.Title;

                            genreTagA.Visibility = Visibility.Visible;
                            genreFind.Text = "";

                            genreElementsPanel.Visibility = Visibility.Visible;
                            genreElementsPanel.Height = double.NaN;

                            genreBool = true;
                        }
                        else if (genreB.Text == "")
                        {
                            if (genreA.Text != genre.Title)
                            {
                                genreB.Text = genre.Title;

                                genreTagB.Visibility = Visibility.Visible;

                                genreFind.Text = "";
                            }
                        }
                        else if (genreC.Text == "")
                        {
                            if (genreA.Text != genre.Title && genreB.Text != genre.Title)
                            {
                                genreC.Text = genre.Title;

                                genreTagC.Visibility = Visibility.Visible;

                                genreFind.Text = "";
                            }
                        }
                        else if (genreC.Text != "")
                        {
                            if (genreA.Text != genre.Title && genreB.Text != genre.Title)
                            {
                                genreC.Text = genre.Title;

                                genreTagC.Visibility = Visibility.Visible;

                                genreFind.Text = "";
                            }
                        }
                    };
                }
            }
            else if (tags != null)
            {
                foreach (var tag in tags.OrderBy(t => t.Title))
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
                        Text = tag.Title,
                        Margin = new Thickness(10, 0, 10, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    authorBorder.Child = textBlock;

                    authorBorder.MouseLeftButtonDown += (sender, e) =>
                    {
                        if (tagA.Text == "")
                        {
                            tagA.Text = tag.Title;

                            tagTagA.Visibility = Visibility.Visible;
                            tagFind.Text = "";

                            tagElementsPanel.Visibility = Visibility.Visible;
                            tagElementsPanel.Height = double.NaN;

                            tagBool = true;
                        }
                        else if (tagB.Text == "")
                        {
                            if (tagA.Text != tag.Title)
                            {
                                tagB.Text = tag.Title;

                                tagTagB.Visibility = Visibility.Visible;

                                tagFind.Text = "";
                            }
                        }
                        else if (tagC.Text == "")
                        {
                            if (tagA.Text != tag.Title && tagB.Text != tag.Title)
                            {
                                tagC.Text = tag.Title;

                                tagTagC.Visibility = Visibility.Visible;

                                tagFind.Text = "";
                            }
                        }
                        else if (tagC.Text != "")
                        {
                            if (tagA.Text != tag.Title && tagB.Text != tag.Title)
                            {
                                tagC.Text = tag.Title;

                                tagTagC.Visibility = Visibility.Visible;

                                tagFind.Text = "";
                            }
                        }
                    };
                }
            }

            return border;
        }
        #endregion

        
        #region Work With Images
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

                    photoBool = true;
                }
                else
                {
                    Path = null;
                    photoBool = false;
                }
            }
        }
        private void ImageOpenClick(object sender, RoutedEventArgs e)
        {
            FindersGotFocus(0);

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                Title = "Выберите изображение обложки"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                Path = openFileDialog.FileName;
                ImageConverter converter = new ImageConverter();
                cover.Source = converter.BitmapImageConvert(Path);
                deleteButton.Visibility = Visibility.Visible;

                photoBool = true;
            }
        }
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (cover.Source != null)
            {
                cover.Source = null;
                Path = null;
                deleteButton.Visibility = Visibility.Hidden;

                photoBool = false;
            }
        }
        #endregion
        
        private void GetBackClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            if (CurrentComic != null)
            {
                NavigationService.Navigate(new ComicEditPage(CurrentComic));
            }
            else
            {
                NavigationService.Navigate(new ComicListPage());
            }
        }

        #region Elements Add Clicks
        private void AuthorAddClick(object sender, RoutedEventArgs e)
        {
            authorAddFrame.Content = new AuthorAdd();
        }
        #endregion

        private void AuthorAddFrameContentRendered(object sender, EventArgs e)
        {
            if (authorAddFrame.Content != null)
            {
                mainGrid.Height = 900;
                splitter.Height = mainBorder.ActualHeight - mainGrid.Margin.Top * 2 - 80;
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
                splitter.Height = mainBorder.ActualHeight - mainGrid.Margin.Top * 2 - 80;
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
            if (AllBool)
            {
                if (CurrentComic != null)
                {
                    Comic comic = DBHelper.DBContext.Comic.Where(c => c.Id_Comic == CurrentComic.Id_Comic).FirstOrDefault();

                    if (Path != null)
                    {
                        comic.Cover = File.ReadAllBytes(Path);
                    }
                    comic.Title = title.Text;
                    comic.Year = (short)Convert.ToInt32(year.Text);
                    comic.Info = info.Text;

                    #region Elements DB Add
                    DBHelper.DBContext.ComicAndAuthor.RemoveRange(DBHelper.DBContext.ComicAndAuthor.Where(c => c.Id_Comic == CurrentComic.Id_Comic));
                    DBHelper.DBContext.ComicAndGenre.RemoveRange(DBHelper.DBContext.ComicAndGenre.Where(c => c.Id_Comic == CurrentComic.Id_Comic));
                    DBHelper.DBContext.ComicAndTag.RemoveRange(DBHelper.DBContext.ComicAndTag.Where(c => c.Id_Comic == CurrentComic.Id_Comic));

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

                    if (genreA.Text != "")
                    {
                        ComicAndGenre comicAndGenreFirst = new ComicAndGenre
                        {
                            Id_Genre = DBHelper.Genres.Where(a => a.Title == genreA.Text).FirstOrDefault().Id_Genre,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndGenre.Add(comicAndGenreFirst);
                        DBHelper.DBContext.SaveChanges();
                    }
                    if (genreB.Text != "")
                    {
                        ComicAndGenre comicAndGenreFirst = new ComicAndGenre
                        {
                            Id_Genre = DBHelper.Genres.Where(a => a.Title == genreB.Text).FirstOrDefault().Id_Genre,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndGenre.Add(comicAndGenreFirst);
                        DBHelper.DBContext.SaveChanges();
                    }
                    if (genreC.Text != "")
                    {
                        ComicAndGenre comicAndGenreFirst = new ComicAndGenre
                        {
                            Id_Genre = DBHelper.Genres.Where(a => a.Title == genreC.Text).FirstOrDefault().Id_Genre,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndGenre.Add(comicAndGenreFirst);
                        DBHelper.DBContext.SaveChanges();
                    }

                    if (tagA.Text != "")
                    {
                        ComicAndTag comicAndTagFirst = new ComicAndTag
                        {
                            Id_Tag = DBHelper.Tags.Where(a => a.Title == tagA.Text).FirstOrDefault().Id_Tag,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndTag.Add(comicAndTagFirst);
                        DBHelper.DBContext.SaveChanges();
                    }
                    if (tagB.Text != "")
                    {
                        ComicAndTag comicAndTagFirst = new ComicAndTag
                        {
                            Id_Tag = DBHelper.Tags.Where(a => a.Title == tagB.Text).FirstOrDefault().Id_Tag,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndTag.Add(comicAndTagFirst);
                        DBHelper.DBContext.SaveChanges();
                    }
                    if (tagC.Text != "")
                    {
                        ComicAndTag comicAndTagFirst = new ComicAndTag
                        {
                            Id_Tag = DBHelper.Tags.Where(a => a.Title == tagC.Text).FirstOrDefault().Id_Tag,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndTag.Add(comicAndTagFirst);
                        DBHelper.DBContext.SaveChanges();
                    }
                    #endregion
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

                    if (genreA.Text != "")
                    {
                        ComicAndGenre comicAndGenreFirst = new ComicAndGenre
                        {
                            Id_Genre = DBHelper.Genres.Where(a => a.Title == genreA.Text).FirstOrDefault().Id_Genre,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndGenre.Add(comicAndGenreFirst);
                        DBHelper.DBContext.SaveChanges();
                    }
                    if (genreB.Text != "")
                    {
                        ComicAndGenre comicAndGenreFirst = new ComicAndGenre
                        {
                            Id_Genre = DBHelper.Genres.Where(a => a.Title == genreB.Text).FirstOrDefault().Id_Genre,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndGenre.Add(comicAndGenreFirst);
                        DBHelper.DBContext.SaveChanges();
                    }
                    if (genreC.Text != "")
                    {
                        ComicAndGenre comicAndGenreFirst = new ComicAndGenre
                        {
                            Id_Genre = DBHelper.Genres.Where(a => a.Title == genreC.Text).FirstOrDefault().Id_Genre,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndGenre.Add(comicAndGenreFirst);
                        DBHelper.DBContext.SaveChanges();
                    }

                    if (tagA.Text != "")
                    {
                        ComicAndTag comicAndTagFirst = new ComicAndTag
                        {
                            Id_Tag = DBHelper.Tags.Where(a => a.Title == tagA.Text).FirstOrDefault().Id_Tag,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndTag.Add(comicAndTagFirst);
                        DBHelper.DBContext.SaveChanges();
                    }
                    if (tagB.Text != "")
                    {
                        ComicAndTag comicAndTagFirst = new ComicAndTag
                        {
                            Id_Tag = DBHelper.Tags.Where(a => a.Title == tagB.Text).FirstOrDefault().Id_Tag,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndTag.Add(comicAndTagFirst);
                        DBHelper.DBContext.SaveChanges();
                    }
                    if (tagC.Text != "")
                    {
                        ComicAndTag comicAndTagFirst = new ComicAndTag
                        {
                            Id_Tag = DBHelper.Tags.Where(a => a.Title == tagC.Text).FirstOrDefault().Id_Tag,
                            Id_Comic = DBHelper.Comics.Where(c => c.Title == comic.Title).FirstOrDefault().Id_Comic
                        };

                        DBHelper.DBContext.ComicAndTag.Add(comicAndTagFirst);
                        DBHelper.DBContext.SaveChanges();
                    }

                    (OtherHelper.MainWindow.mainFrame.Content as ViewPort).numOfComics.Text = DBHelper.CurrentUser.Comic.Count.ToString();
                }
                GetBackClick(sender, e);
            }
            else
            {
                ComicMessageBox comicMessageBox = new ComicMessageBox("Внимание!", "Сначала правильно заполните все поля")
                {
                    Owner = OtherHelper.MainWindow
                };
                comicMessageBox.ShowDialog();
            }
        }


        #region Got Focus
        private void FindersGotFocus(int a)
        {
            if (a != 0)
            {
                mainGrid.Height = 900;
                splitter.Height = mainBorder.ActualHeight - mainGrid.Margin.Top * 2 - 80;
                splitter.VerticalAlignment = VerticalAlignment.Top;
            }

            if (a != 1)
            {
                listGrid.Visibility = Visibility.Hidden;
                listGrid.Height = 0;
            }
            if (a != 2)
            {
                listGenre.Visibility = Visibility.Hidden;
                listGenre.Height = 0;
            }
            if (a != 3)
            {
                listTags.Visibility = Visibility.Hidden;
                listTags.Height = 0;
            }
        }
        private void AuthorFindGotFocus(object sender, RoutedEventArgs e)
        {
            listGrid.Visibility = Visibility.Visible;
            listGrid.Height = double.NaN;
            FindersGotFocus(1);
        }
        private void GenreFindGotFocus(object sender, RoutedEventArgs e)
        {
            listGenre.Visibility = Visibility.Visible;
            listGenre.Height = double.NaN;
            FindersGotFocus(2);
        }
        private void TagFindGotFocus(object sender, RoutedEventArgs e)
        {
            listTags.Visibility = Visibility.Visible;
            listTags.Height = double.NaN;
            FindersGotFocus(3);
        }
        #endregion


        private void ListsIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (listGrid.IsVisible)
            {
                tagBack.Visibility = Visibility.Hidden;
                tagBack.Height = 0;
                tagElementsPanel.Height = 0;
                tagElementsPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                tagBack.Visibility = Visibility.Visible;
                tagBack.Height = double.NaN;
                tagElementsPanel.Height = double.NaN;
                tagElementsPanel.Visibility = Visibility.Visible;
            }

            if (listGenre.IsVisible)
            {
                authorBack.Visibility = Visibility.Hidden;
                authorBack.Height = 0;
                authorElementsPanel.Height = 0;
                authorElementsPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                authorBack.Visibility = Visibility.Visible;
                authorBack.Height = double.NaN;
                authorElementsPanel.Height = double.NaN;
                authorElementsPanel.Visibility = Visibility.Visible;
            }

            if (listTags.IsVisible)
            {
                genreBack.Visibility = Visibility.Hidden;
                genreBack.Height = 0;
                genreElementsPanel.Height = 0;
                genreElementsPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                genreBack.Visibility = Visibility.Visible;
                genreBack.Height = double.NaN;
                genreElementsPanel.Height = double.NaN;
                genreElementsPanel.Visibility = Visibility.Visible;
            }
        }


        #region Finder Text Changes
        private void AuthorFindTextChanged(object sender, TextChangedEventArgs e)
        {
            listGrid.Children.Clear();
            if (authorFind.Text == "")
            {
                listGrid.Children.Add(ListCreate(DBHelper.Authors, null, null));
                listGrid.Children.Add(AuthorsNoElementText);

                AuthorsNoElementText.Visibility = Visibility.Hidden;
            }
            else
            {
                List<Author> authors = DBHelper.Authors.Where(a => a.Name.ToLower().Contains(authorFind.Text.ToLower())).ToList();

                listGrid.Children.Add(ListCreate(authors, null, null));
                listGrid.Children.Add(AuthorsNoElementText);

                if (authors.Count == 0)
                {
                    AuthorsNoElementText.Visibility = Visibility.Visible;
                }
                else
                {
                    AuthorsNoElementText.Visibility = Visibility.Hidden;
                }
            }
        }

        private void GenreFindTextChanged(object sender, TextChangedEventArgs e)
        {
            listGenre.Children.Clear();
            if (genreFind.Text == "")
            {
                listGenre.Children.Add(ListCreate(null, DBHelper.Genres, null));
                listGenre.Children.Add(GenresNoElementText);

                GenresNoElementText.Visibility = Visibility.Hidden;
            }
            else
            {
                List<Genre> genres = DBHelper.Genres.Where(a => a.Title.ToLower().Contains(genreFind.Text.ToLower())).ToList();
                listGenre.Children.Add(ListCreate(null, genres, null));
                listGenre.Children.Add(GenresNoElementText);

                if (genres.Count == 0)
                {
                    GenresNoElementText.Visibility = Visibility.Visible;
                }
                else
                {
                    GenresNoElementText.Visibility = Visibility.Hidden;
                }
            }
        }

        private void TagFindTextChanged(object sender, TextChangedEventArgs e)
        {
            listTags.Children.Clear();
            if (tagFind.Text == "")
            {
                listTags.Children.Add(ListCreate(null, null, DBHelper.Tags));
                listTags.Children.Add(TagsNoElementText);

                TagsNoElementText.Visibility = Visibility.Hidden;
            }
            else
            {
                List<Tag> tags = DBHelper.Tags.Where(a => a.Title.ToLower().Contains(tagFind.Text.ToLower())).ToList();
                listTags.Children.Add(TagsNoElementText);
                listTags.Children.Add(ListCreate(null, null, tags));

                if (tags.Count == 0)
                {
                    TagsNoElementText.Visibility = Visibility.Visible;
                }
                else
                {
                    TagsNoElementText.Visibility = Visibility.Hidden;
                }
            }
        }
        #endregion


        #region Elements Removes
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

                authorElementsPanel.Visibility = Visibility.Hidden;
                authorElementsPanel.Height = 0;

                authorA.Text = "";
                authorFind.Text = "";

                authorBool = false;
            }
        }
        private void AuthorBRemoveClick(object sender, RoutedEventArgs e)
        {
            authorTagB.Visibility = Visibility.Hidden;
            authorB.Text = "";

            authorFind.Text = "";
            authorFind.IsEnabled = true;
        }


        private void GenreARemoveClick(object sender, RoutedEventArgs e)
        {
            if (genreB.Text != "" && genreC.Text != "")
            {
                genreA.Text = genreB.Text;
                genreB.Text = genreC.Text;

                genreC.Text = "";
                genreTagC.Visibility = Visibility.Hidden;

                genreFind.Text = "";
            }
            else if (genreB.Text != "" && genreC.Text == "")
            {
                genreA.Text = genreB.Text;

                genreB.Text = "";
                genreTagB.Visibility = Visibility.Hidden;

                genreFind.Text = "";
            }
            else
            {
                genreTagA.Visibility = Visibility.Hidden;

                genreElementsPanel.Visibility = Visibility.Hidden;
                genreElementsPanel.Height = 0;

                genreA.Text = "";
                genreFind.Text = "";

                genreBool = false;
            }
        }
        private void GenreBRemoveClick(object sender, RoutedEventArgs e)
        {
            if (genreC.Text != "")
            {
                genreB.Text = genreC.Text;

                genreC.Text = "";
                genreTagC.Visibility = Visibility.Hidden;

                genreFind.IsEnabled = true;
                genreFind.Text = "";
            }
            else
            {
                genreTagB.Visibility = Visibility.Hidden;

                genreFind.IsEnabled = true;
                genreB.Text = "";
                genreFind.Text = "";
            }
        }
        private void GenreCRemoveClick(object sender, RoutedEventArgs e)
        {
            genreTagC.Visibility = Visibility.Hidden;
            genreC.Text = "";

            genreFind.Text = "";
            genreFind.IsEnabled = true;
        }


        private void TagARemoveClick(object sender, RoutedEventArgs e)
        {
            if (tagB.Text != "" && tagC.Text != "")
            {
                tagA.Text = tagB.Text;
                tagB.Text = tagC.Text;

                tagC.Text = "";
                tagTagC.Visibility = Visibility.Hidden;

                tagFind.Text = "";
            }
            else if (tagB.Text != "" && tagC.Text == "")
            {
                tagA.Text = tagB.Text;

                tagB.Text = "";
                tagTagB.Visibility = Visibility.Hidden;

                tagFind.Text = "";
            }
            else
            {
                tagTagA.Visibility = Visibility.Hidden;

                tagElementsPanel.Visibility = Visibility.Hidden;
                tagElementsPanel.Height = 0;

                tagA.Text = "";
                tagFind.Text = "";

                tagBool = false;
            }
        }
        private void TagBRemoveClick(object sender, RoutedEventArgs e)
        {
            if (tagC.Text != "")
            {
                tagB.Text = tagC.Text;

                tagC.Text = "";
                tagTagC.Visibility = Visibility.Hidden;

                tagFind.IsEnabled = true;
                tagFind.Text = "";
            }
            else
            {
                tagTagB.Visibility = Visibility.Hidden;

                tagB.Text = "";
                tagFind.Text = "";
            }
        }
        private void TagCRemoveClick(object sender, RoutedEventArgs e)
        {
            tagTagC.Visibility = Visibility.Hidden;
            tagC.Text = "";

            tagFind.Text = "";
            tagFind.IsEnabled = true;
        }
        #endregion


        #region Year Text Box Work
        private void YearTextChanged(object sender, TextChangedEventArgs e)
        {
            FindersGotFocus(0);

            if (OtherHelper.IsNotValidText(year.Text, "цифры", "[0-9]", out string mes))
            {
                yearMSG.Text = mes;
                yearMSG.Foreground = Brushes.Red;

                yearBool = false;
            }
            else if (Convert.ToInt32(year.Text) < 1900 || Convert.ToInt32(year.Text) > DateTime.Now.Year)
            {
                yearMSG.Text = "Неверный формат года";
                yearMSG.Foreground = Brushes.Red;

                yearBool = false;
            }
            else
            {
                yearMSG.Text = "Введите год выпуска комикса";
                yearMSG.Foreground = Brushes.Black;

                yearBool = true;
            }
        }
        private void YearPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, "[0-9]").Success)
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Info and Title Text Box Work
        private void InfoTextChanged(object sender, TextChangedEventArgs e)
        {
            if (OtherHelper.IsNotValidText(info.Text, "кирилица/латиница и символы", @"[а-яА-Яa-zA-Z0-9ё\s\042№«»!?/':.,—-]", out string mes))
            {
                infoMSG.Text = mes;
                infoMSG.Foreground = Brushes.Red;
                
                infoBool = false;
            }
            else
            {
                infoMSG.Text = "Введите краткое описание комикса";
                infoMSG.Foreground = Brushes.Black;

                infoBool = true;
            }
        }
        private void TitleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (OtherHelper.IsNotValidText(title.Text, "кирилица/латиница и символы", @"[а-яА-Яa-zA-Z0-9ё\s\042№«»!?/':.,—-]", out string mes))
            {
                titleMSG.Text = mes;
                titleMSG.Foreground = Brushes.Red;

                titleBool = false;
            }
            else
            {
                titleMSG.Text = "Введите название будующего комикса";
                titleMSG.Foreground = Brushes.Black;

                titleBool = true;
            }
        }

        private void RegularTextPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[а-яА-Яa-zA-Z0-9ё\s\042№«»!?/':.,—-]").Success)
            {
                e.Handled = true;
            }
        }
        #endregion
    }
}
