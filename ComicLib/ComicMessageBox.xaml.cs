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
using System.Windows.Shapes;
using ComicLib.ClassHelpers;

namespace ComicLib
{
    /// <summary>
    /// Логика взаимодействия для ComicMessageBox.xaml
    /// </summary>
    public partial class ComicMessageBox : Window
    {
        public string Message { get; set; }
        public ComicMessageBox(string headerMessage, string message, bool a = false)
        {
            InitializeComponent();

            if (a)
                notOk.Visibility = Visibility.Visible;
            else
                notOk.Visibility = Visibility.Hidden;

            Opacity = 0;

            Message = message;
            mes.Text = message;
            headerText.Text = headerMessage;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void WindowCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            OtherHelper.OkOrNot = true;
            this.Close();
        }

        private void NotOkClick(object sender, RoutedEventArgs e)
        {
            OtherHelper.OkOrNot = false;
            this.Close();
        }
    }
}
