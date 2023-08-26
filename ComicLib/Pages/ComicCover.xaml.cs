using ComicLib.ClassHelpers;
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
    /// Логика взаимодействия для ComicCover.xaml
    /// </summary>
    public partial class ComicCover : System.Windows.Controls.Page
    {
        private Comic Comic { get; set; }
        public ComicCover(Comic comic)
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
        }

        private void PageMouseDown(object sender, MouseButtonEventArgs e)
        {
            (OtherHelper.MainWindow.mainFrame.Content as ViewPort).ChangeVisible(0);
        }
    }
}
