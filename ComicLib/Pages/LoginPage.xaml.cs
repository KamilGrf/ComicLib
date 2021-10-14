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
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            string mes = "";
            if (name.Text == "" || pas.Text == "")
            {
                MessageBox.Show("Поля не должны оставаться пустыми");
            }
            else if (DBHelper.GetUser(name.Text, pas.Text, out mes))
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
                NavigationService.Navigate(new ViewPort());
            }
            else
            {
                MessageBox.Show(mes);
            }


            //if (NavigationService.CanGoBack)
            //{
            //    NavigationService.RemoveBackEntry();
            //}
            //NavigationService.Navigate(new ViewPort());
        }

        private void RegisterPageClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new RegisterPage());
        }
    }
}
