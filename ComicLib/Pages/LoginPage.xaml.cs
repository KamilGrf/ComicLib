using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComicLib.Animations;
using ComicLib.ClassHelpers;
using System.Text.RegularExpressions;
using ComicLib.Properties;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : BasePage
    {
        public LoginPage()
        {
            InitializeComponent();

            if (NavigationService != null)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
            DBHelper.LoginForget();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (login.Text == "" || pas.Text == "")
            {
                errorMes.Text = "Все поля должны быть заполнены!";
            }
            else if (DBHelper.GetUser(login.Text, pas.Text, out string mes))
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
                NavigationService.Navigate(new ViewPort());
            }
            else
            {
                errorMes.Text = mes;
            }
        }

        private void RegisterPageClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new RegisterPage());
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
    }
}
