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

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComicListPage.xaml
    /// </summary>
    public partial class ComicListPage : System.Windows.Controls.Page
    {
        public double ComicListWidth { get { return mainPage.ActualWidth - (dp.Margin.Left + dp.Margin.Right) - filterBorder.ActualWidth - filterBorder.Margin.Left; } }
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

            //List<Comic> w = new List<Comic>();
            //for (int i = 0; i < 50; i++)
            //{
            //    w.Add(DBHelper.DBContext.Comic.ToList()[4]);
            //}

            comicList.ItemsSource = DBHelper.DBContext.Comic.ToList();
            //comicList.ItemsSource = w;
        }

        private void MainPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            comicList.Width = ComicListWidth;
        }
    }
}
