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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComicLib.ClassHelpers;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : BasePage
    {
        public RegisterPage()
        {
            InitializeComponent();

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
        }

        private void RegClick(object sender, RoutedEventArgs e)
        {
            if (login.Text.Length == 0 || email.Text.Length == 0 || pas.Text.Length == 0)
            {
                errorMes.Text = "Все поля должны быть заполнены!";
            }
            else if (DBHelper.ValidateEmail(email.Text, out string message) && PasswordHelper.ValidatePassword(pas.Text, out message))
            {
                User user = new User
                {
                    Name = login.Text,
                    Email = email.Text,
                    Password = pas.Text
                };
                DBHelper.DBContext.User.Add(user);
                DBHelper.DBContext.SaveChanges();

                //DBHelper.CurrentUser = DBHelper.DBContext.User.Where(u => u.Name == login.Text && u.Password == pas.Text).FirstOrDefault();
                DBHelper.LoginRemember(login.Text, pas.Text);

                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
                NavigationService.Navigate(new ViewPort());
            }
            else
            {
                errorMes.Text = message;
            }
        }

        private void LoginPageClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new LoginPage());
        }

        private void TextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private void LoginPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[0-9a-zA-Zа-яА-Я]").Success)
            {
                e.Handled = true;
            }
        }

        private void PasswordPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[0-9a-zA-Z!@#$%^&*()_+=\[{\]};:<>|./?,-]").Success)
            {
                e.Handled = true;
            }
        }

        private void EmailPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[a-z0-9@!#$%&.'*+/=?^_`{|}~-]").Success)
            {
                e.Handled = true;
            }
        }
    }
}
