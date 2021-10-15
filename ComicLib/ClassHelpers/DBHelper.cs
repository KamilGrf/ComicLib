using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ComicLib.ClassHelpers
{
    /// <summary>
    /// Предоставляет общие данные из базы данных
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// Хранилище подключенной базы данных
        /// </summary>
        public static ComicLibEntities DBContext { get; set; } = new ComicLibEntities();

        public static BitmapImage Image { get; set; }
        public static User CurrentUser { get; set; }

        public static List<Author> Authors { get; set; }


        /// <summary>
        /// Проверяет наличие учетной записи.
        /// Функция получения пользователя из базы данных
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pas"></param>
        /// <param name="message"></param>
        /// <returns>Значение true, если учетная запись найдена; в противном случае - значение false</returns>
        public static bool GetUser(string login, string pas, out string message)
        {
            CurrentUser = DBContext.User.Where(u => u.Name == login && u.Password == pas).FirstOrDefault();

            if (CurrentUser != null)
            {
                message = "";
                return true;
            }
            else if (DBContext.User.Where(u => u.Name == login).FirstOrDefault() != null)
            {
                message = "Неверный пароль!";
                return false;
            }
            else
            {
                message = "Неверный логин!";
                return false;
            }
        }
    }
}
