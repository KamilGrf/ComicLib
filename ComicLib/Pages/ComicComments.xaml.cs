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
using MaterialDesignThemes.Wpf;
using ComicLib.ClassHelpers;
using System.Security.Principal;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComicComments.xaml
    /// </summary>
    public partial class ComicComments : System.Windows.Controls.Page
    {
        private List<Comment> Comments { get; set; } = new List<Comment>();
        private Comic Comic { get; set; }
        public ComicComments(Comic comic)
        {
            InitializeComponent();

            Comic = comic;
            DataContext = comic;

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            Comments = DBHelper.DBContext.Comic.Where(c => c.Id_Comic == comic.Id_Comic).FirstOrDefault().Comment.OrderByDescending(c => c.Date).ToList();
            if (Comments.Count == 0)
            {
                noComments.Visibility = Visibility.Visible;
            }
            else
            {
                CommentsListCreate(Comments);
                noComments.Visibility = Visibility.Hidden;
            }
        }

        private void CommentsListCreate(List<Comment> comments)
        {
            commentsStack.Children.Clear();

            foreach (var comment in comments)
            {
                BitmapImage bImage = new BitmapImage();
                if (comment.User.Avatar != null)
                {
                    MemoryStream byteStream = new MemoryStream(comment.User.Avatar);
                    bImage.BeginInit();
                    bImage.CacheOption = BitmapCacheOption.OnLoad;
                    bImage.StreamSource = byteStream;
                    bImage.EndInit();
                }

                Border border = new Border
                {
                    Margin = new Thickness(0, 10, 0, 10),
                    Padding = new Thickness(10),
                    CornerRadius = new CornerRadius(4),
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.LightGray
                };
                commentsStack.Children.Add(border);

                StackPanel mainStackPanel = new StackPanel();
                border.Child = mainStackPanel;

                StackPanel profileStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Brushes.Transparent
                };
                mainStackPanel.Children.Add(profileStackPanel);

                if (comment.User.Avatar != null)
                {
                    Image avatar = new Image
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        Height = 25,
                        Width = 25,
                        Margin = new Thickness(0, 0, 5, 0),
                        Stretch = Stretch.Fill,
                        Source = bImage
                    };
                    profileStackPanel.Children.Add(avatar);
                }
                else
                {
                    PackIcon avatar = new PackIcon
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0, 5, 0),
                        Kind = PackIconKind.Account
                    };
                    profileStackPanel.Children.Add(avatar);
                }
                TextBlock userName = new TextBlock
                {
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Text = comment.User.Name
                };
                profileStackPanel.Children.Add(userName);

                TextBlock commentText = new TextBlock
                {
                    Margin = new Thickness(0, 10, 0, 10),
                    TextWrapping = TextWrapping.Wrap,
                    Text = comment.Text
                };
                mainStackPanel.Children.Add(commentText);

                Grid grid = new Grid();
                mainStackPanel.Children.Add(grid);

                TextBlock date = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 12,
                    Opacity = 0.6,
                    Text = comment.Date.Value.ToLongDateString()
                };
                grid.Children.Add(date);


                StackPanel like = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 0, 30, 5),
                    Background = Brushes.Transparent
                };
                grid.Children.Add(like);


                PackIcon packIcon = new PackIcon
                {
                    Kind = PackIconKind.LikeOutline,
                    VerticalAlignment = VerticalAlignment.Center
                };
                like.Children.Add(packIcon);
                TextBlock ratingText = new TextBlock
                {
                    Margin = new Thickness(5, 0, 5, 0),
                    Text = comment.Rating.ToString()
                };
                like.Children.Add(ratingText);

                like.MouseDown += (e, sender) =>
                {
                    if (packIcon.Kind != PackIconKind.Like)
                    {
                        Comment com = DBHelper.DBContext.Comment.Where(c => c.Id_Comment == comment.Id_Comment).FirstOrDefault();

                        com.Rating = (short)((int)comment.Rating + 1);
                        ratingText.Text = com.Rating.ToString();
                        packIcon.Foreground = Brushes.Green;
                        packIcon.Kind = PackIconKind.Like;

                        DBHelper.DBContext.SaveChanges();
                    }
                    else
                    {
                        Comment com = DBHelper.DBContext.Comment.Where(c => c.Id_Comment == comment.Id_Comment).FirstOrDefault();

                        com.Rating = (short)((int)comment.Rating - 1);
                        ratingText.Text = com.Rating.ToString();
                        packIcon.Foreground = Brushes.Black;
                        packIcon.Kind = PackIconKind.LikeOutline;

                        DBHelper.DBContext.SaveChanges();
                    }
                };


                like.MouseEnter += (e, sender) =>
                {
                    Cursor = Cursors.Hand;
                };
                like.MouseLeave += (e, sender) =>
                {
                    Cursor = Cursors.Arrow;
                };
            }
        }


        private void CommentTextChanged(object sender, TextChangedEventArgs e)
        {
            if (comment.Text == "")
                commentBackText.Visibility = Visibility.Visible;
            else
                commentBackText.Visibility = Visibility.Hidden;
        }

        private void SortClick(object sender, RoutedEventArgs e)
        {
            if (sortBack.IsVisible)
                sortBack.Visibility = Visibility.Hidden;
            else
                sortBack.Visibility = Visibility.Visible;
        }

        private void PageMouseDown(object sender, MouseButtonEventArgs e)
        {
            sortBack.Visibility = Visibility.Hidden;
            (OtherHelper.MainWindow.mainFrame.Content as ViewPort).ChangeVisible(0);
        }

        #region Sorting
        private void SortNewClick(object sender, RoutedEventArgs e)
        {
            sort.Text = "Новые";
            sortBack.Visibility = Visibility.Hidden;
            CommentsListCreate(Comments.OrderByDescending(c => c.Date).ToList());
        }
        private void SortOldClick(object sender, RoutedEventArgs e)
        {
            sort.Text = "Старые";
            sortBack.Visibility = Visibility.Hidden;
            CommentsListCreate(Comments.OrderBy(c => c.Date).ToList());
        }
        private void SortPopClick(object sender, RoutedEventArgs e)
        {
            sort.Text = "Популярные";
            sortBack.Visibility = Visibility.Hidden;
            CommentsListCreate(Comments.OrderByDescending(c => c.Rating).ToList());
        }
        #endregion

        private void CommentAddClick(object sender, RoutedEventArgs e)
        {
            if (comment.Text == "")
            {

            }
            else
            {
                Comment newComment = new Comment
                {
                    Date = DateTime.Now,
                    Text = comment.Text,
                    Id_User = DBHelper.CurrentUser.Id_User,
                    Id_Comic = Comic.Id_Comic,
                    Rating = 0
                };

                DBHelper.DBContext.Comment.Add(newComment);
                DBHelper.DBContext.SaveChanges();

                noComments.Visibility = Visibility.Hidden;
                comment.Text = "";

                Comments = DBHelper.DBContext.Comic.Where(c => c.Id_Comic == Comic.Id_Comic).FirstOrDefault().Comment.OrderByDescending(c => c.Date).ToList();
                CommentsListCreate(Comments);
            }
        }
    }
}
