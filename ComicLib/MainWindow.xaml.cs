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
using ComicLib.Pages;

namespace ComicLib
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //mainFrame.NavigationService.Navigate(new ViewPort());
            mainFrame.NavigationService.Navigate(new LoginPage());
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void MenuClick(object sender, RoutedEventArgs e)
        {
            Point pointToWindow = Mouse.GetPosition(this);
            Point pointToScreen = PointToScreen(pointToWindow);

            //SystemCommands.ShowSystemMenu(this, Mouse.GetPosition(Application.Current.MainWindow));
            SystemCommands.ShowSystemMenu(this, pointToScreen);
        }

        private void GridDragMoveMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            DragMove();
        }
    }
}
