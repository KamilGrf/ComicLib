using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComicLib.ClassHelpers
{
    /// <summary>
    /// Предоставляет общие полезные данные программы
    /// </summary>
    public static class OtherHelper
    {
        public static MainWindow MainWindow { get { return Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(); } }

    }
}
