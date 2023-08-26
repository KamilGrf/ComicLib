using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComicLib.ClassHelpers;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComicListPage.xaml
    /// </summary>
    public partial class ComicListPage : System.Windows.Controls.Page
    {
        private const string any = "Любые";

        private bool isGenre;
        private bool isTag;
        private bool isAuthor;

        private bool isNumFrom;
        private bool isNumTo;
        private bool isYearFrom;
        private bool isYearTo;

        public ComicListPage()
        {
            InitializeComponent();
            
            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            genreFilterList.ItemsSource = DBHelper.Genres.ToList();
            tagFilterList.ItemsSource = DBHelper.Tags.ToList();
            authorFilterList.ItemsSource = DBHelper.DBContext.Author.OrderBy(a => a.Name).ToList();

            comicList.ItemsSource = DBHelper.DBContext.Comic.OrderBy(a => a.Title).ToList();
        }

        private void ComicListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comicList.SelectedItem != null)
            {
                NavigationService.Content = null;

                NavigationService.Navigate(new ComicInfo(comicList.SelectedItem as Comic, false));

                comicList.ItemsSource = null;
            }
        }


        #region Toggle Button Filters
        private void GenreToggleButtonChecked(object sender, RoutedEventArgs e)
        {
        }
        private void TagToggleButtonChecked(object sender, RoutedEventArgs e)
        {
        }
        private void AuthorToggleButtonChecked(object sender, RoutedEventArgs e)
        {
        }
        #endregion


        #region Page Count Range
        private void NumFromTextChanged(object sender, TextChangedEventArgs e)
        {
            if (numFrom.Text.Length != 0)
            {
                isNumFrom = true;
                numFromBack.Visibility = Visibility.Hidden;
            }
            else
            {
                isNumFrom = false;
                numFromBack.Visibility = Visibility.Visible;
            }
        }
        private void NumToTextChanged(object sender, TextChangedEventArgs e)
        {
            if (numTo.Text.Length != 0)
            {
                isNumTo = true;
                numToBack.Visibility = Visibility.Hidden;
            }
            else
            {
                isNumTo = false;
                numToBack.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region Year Range
        private void YearFromTextChanged(object sender, TextChangedEventArgs e)
        {
            if (yearFrom.Text.Length != 0)
            {
                isYearFrom = true;
                yearFromBack.Visibility = Visibility.Hidden;
            }
            else
            {
                isYearFrom = false;
                yearFromBack.Visibility = Visibility.Visible;
            }
        }
        private void YearToTextChanged(object sender, TextChangedEventArgs e)
        {
            if (yearTo.Text.Length != 0)
            {
                isYearTo = true;
                yearToBack.Visibility = Visibility.Hidden;
            }
            else
            {
                isYearTo = false;
                yearToBack.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region Finder
        private void ComicFindTextChanged(object sender, TextChangedEventArgs e)
        {
            if (comicFind.Text.Length != 0)
                findBack.Visibility = Visibility.Hidden;
            else
                findBack.Visibility = Visibility.Visible;
        }
        private void FindClick(object sender, RoutedEventArgs e)
        {
            DataReset();
            if (comicFind.Text.Length != 0)
            {
                comicList.ItemsSource = DBHelper.Comics.Where(c => c.Title.ToLower().Contains(comicFind.Text.ToLower())).ToList();
            }

            if (comicList.Items.Count == 0)
                noElements.Visibility = Visibility.Visible;
            else
                noElements.Visibility = Visibility.Hidden;
        }
        #endregion


        private void FilterBackClick(object sender, RoutedEventArgs e)
        {
            genreButton.IsChecked = false;
            tagButton.IsChecked = false;
            authorButton.IsChecked = false;

            if (genreFilterList.SelectedItems.Count > 0)
            {
                genres.Text = "";
                for (int i = 0; i < genreFilterList.SelectedItems.Count; i++)
                {
                    if (i == genreFilterList.SelectedItems.Count - 1)
                        genres.Text += (genreFilterList.SelectedItems[i] as Genre).Title;
                    else
                        genres.Text += (genreFilterList.SelectedItems[i] as Genre).Title + ", ";
                }
                isGenre = true;
            }
            else
            {
                isGenre = false;
                genres.Text = any;
            }

            if (tagFilterList.SelectedItems.Count > 0)
            {
                tags.Text = "";
                for (int i = 0; i < tagFilterList.SelectedItems.Count; i++)
                {
                    if (i == tagFilterList.SelectedItems.Count - 1)
                        tags.Text += (tagFilterList.SelectedItems[i] as Tag).Title;
                    else
                        tags.Text += (tagFilterList.SelectedItems[i] as Tag).Title + ", ";
                }
                isTag = true;
            }
            else
            {
                isTag = false;
                tags.Text = any;
            }

            if (authorFilterList.SelectedItems.Count > 0)
            {
                authors.Text = "";
                for (int i = 0; i < authorFilterList.SelectedItems.Count; i++)
                {
                    if (i == authorFilterList.SelectedItems.Count - 1)
                        authors.Text += (authorFilterList.SelectedItems[i] as Author).Name;
                    else
                        authors.Text += (authorFilterList.SelectedItems[i] as Author).Name + ", ";
                }
                isAuthor = true;
            }
            else
            {
                isAuthor = false;
                authors.Text = any;
            }
        }


        #region List Box Limit
        private void GenreFilterListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (genreFilterList.SelectedItems.Count == 4)
            {
                genreFilterList.SelectedItems.RemoveAt(0);
            }
        }
        private void TagFilterListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tagFilterList.SelectedItems.Count == 4)
            {
                tagFilterList.SelectedItems.RemoveAt(0);
            }
        }
        private void AuthorFilterListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (authorFilterList.SelectedItems.Count == 3)
            {
                authorFilterList.SelectedItems.RemoveAt(0);
            }
        }
        #endregion


        private void DataReset()
        {
            if (authorFilterList.SelectedItems.Count != 0)
            {
                authorFilterList.SelectedItems.Clear();
            }
            if (genreFilterList.SelectedItems.Count != 0)
            {
                genreFilterList.SelectedItems.Clear();
            }
            if (tagFilterList.SelectedItems.Count != 0)
            {
                tagFilterList.SelectedItems.Clear();
            }

            tags.Text = any;
            isTag = false;

            genres.Text = any;
            isGenre = false;

            authors.Text = any;
            isAuthor = false;


            numFrom.Text = "";
            isNumFrom = false;

            numTo.Text = "";
            isNumTo = false;

            yearFrom.Text = "";
            isYearFrom = false;

            yearTo.Text = "";
            isYearTo = false;
        }
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            DataReset();
            comicFind.Text = "";
            comicList.ItemsSource = DBHelper.Comics;
            noElements.Visibility = Visibility.Hidden;
        }

        //
        public void FilterCreator()
        {
            comicList.ItemsSource = DBHelper.Comics;
            if (isGenre)
            {
                List<Comic> comics = new List<Comic>();

                foreach (var comic in comicList.ItemsSource as List<Comic>)
                {
                    int score = 0;
                    foreach (var genre in genreFilterList.SelectedItems)
                    {
                        foreach (var comicAndGenre in comic.ComicAndGenre)
                        {
                            if (comicAndGenre.Genre.Title == (genre as Genre).Title)
                            {
                                score++;
                                if (score == genreFilterList.SelectedItems.Count)
                                {
                                    comics.Add(comic);
                                }
                            }
                        }
                    }
                }
                comicList.ItemsSource = comics;
            }
            if (isTag)
            {
                List<Comic> comics = new List<Comic>();
                foreach (var comic in comicList.ItemsSource as List<Comic>)
                {
                    int score = 0;
                    foreach (var tag in tagFilterList.SelectedItems)
                    {
                        foreach (var comicAndTag in comic.ComicAndTag)
                        {
                            if (comicAndTag.Tag.Title == (tag as Tag).Title)
                            {
                                score++;
                                if (score == tagFilterList.SelectedItems.Count)
                                {
                                    comics.Add(comic);
                                }
                            }
                        }
                    }
                }
                comicList.ItemsSource = comics;
            }
            if (isAuthor)
            {
                List<Comic> comics = new List<Comic>();
                foreach (var comic in comicList.ItemsSource as List<Comic>)
                {
                    int score = 0;
                    foreach (var author in authorFilterList.SelectedItems)
                    {
                        foreach (var comicAndAuthor in comic.ComicAndAuthor)
                        {
                            if (comicAndAuthor.Author.Name == (author as Author).Name)
                            {
                                score++;
                                if (score == authorFilterList.SelectedItems.Count)
                                {
                                    comics.Add(comic);
                                }
                            }
                        }
                    }
                }
                comicList.ItemsSource = comics;
            }
            if (isNumFrom)
            {
                comicList.ItemsSource = (comicList.ItemsSource as List<Comic>).Where(c => c.Page.Count >= Convert.ToInt32(numFrom.Text)).ToList();
            }
            if (isNumTo)
            {
                comicList.ItemsSource = (comicList.ItemsSource as List<Comic>).Where(c => c.Page.Count <= Convert.ToInt32(numTo.Text)).ToList();
            }
            if (isYearFrom)
            {
                comicList.ItemsSource = (comicList.ItemsSource as List<Comic>).Where(c => c.Year >= Convert.ToInt32(yearFrom.Text)).ToList();
            }
            if (isYearTo)
            {
                comicList.ItemsSource = (comicList.ItemsSource as List<Comic>).Where(c => c.Year <= Convert.ToInt32(yearTo.Text)).ToList();
            }

            if (comicList.Items.Count == 0)
                noElements.Visibility = Visibility.Visible;
            else
                noElements.Visibility = Visibility.Hidden;
        }

        private void ShowClick(object sender, RoutedEventArgs e)
        {
            comicFind.Text = "";
            FilterCreator();
        }

        private void NumFromPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[0-9]").Success)
            {
                e.Handled = true;
            }
        }
        private void TextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }
    }
}
