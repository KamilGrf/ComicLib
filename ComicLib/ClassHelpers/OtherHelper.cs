using ComicLib.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ComicLib.ClassHelpers
{
    /// <summary>
    /// Предоставляет общие полезные данные программы
    /// </summary>
    public static class OtherHelper
    {
        public static MainWindow MainWindow { get { return Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(); } }

        public static bool OkOrNot { get; set; } = true;

        public static bool IsNotValidText(string text, string keyWord, string pattern, out string msg)
        {
            if (text == "")
            {
                msg = "Заполните поле";
                return true;
            }
            else if (Regex.Matches(text, pattern).Count != text.Length)
            {
                msg = "Только " + keyWord;
                return true;
            }
            else
            {
                msg = "";
                return false;
            }
        }
    }
}
