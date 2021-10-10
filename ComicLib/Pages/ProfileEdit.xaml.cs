using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
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
using static System.Net.Mime.MediaTypeNames;

namespace ComicLib.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileEdit.xaml
    /// </summary>
    public partial class ProfileEdit : Page
    {
        public ProfileEdit()
        {
            InitializeComponent();
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                if (path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
                {
                    ImageConverter converter = new ImageConverter();
                    BitmapImage image = converter.BitmapImageConvert(path);

                    //ava.Source = new BitmapImage(new Uri(path));
                    ava.Source = image;
                }
            }
            //(string[])e.Data.GetData(DataFormats.FileDrop)
        }
    }
}
