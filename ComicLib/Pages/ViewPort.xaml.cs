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

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewPort.xaml
    /// </summary>
    public partial class ViewPort : Page
    {
        public ViewPort()
        {
            InitializeComponent();
            profileMenu.Visibility = Visibility.Hidden;
        }

        private void ProfileIconClick(object sender, RoutedEventArgs e)
        {
            if (profileMenu.IsVisible)
                profileMenu.Visibility = Visibility.Hidden;
            else
                profileMenu.Visibility = Visibility.Visible;
        }


        #region Profile Menu Button Clicks
        private void ProfileClick(object sender, RoutedEventArgs e)
        {

        }

        private void MineBookmarksClick(object sender, RoutedEventArgs e)
        {

        }

        private void MineCommentsClick(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {

        }

        private void AccLeaveClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
