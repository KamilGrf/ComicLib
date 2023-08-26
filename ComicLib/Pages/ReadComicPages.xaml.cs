using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReadComicPages.xaml
    /// </summary>
    public partial class ReadComicPages : System.Windows.Controls.Page
    {
        private Comic Comic { get; set; }
        public ReadComicPages(Comic comic)
        {
            InitializeComponent();

            Comic = comic;

            pages.ItemsSource = comic.Page.ToList();
        }

        private void GoBackClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
            NavigationService.Navigate(new ComicInfo(Comic, false));
        }

        private void PageUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            pages.ItemsSource = null;
            mainGrid.Children.Clear();
        }
    }
}
