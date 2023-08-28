using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ComicLib.Properties;

namespace ComicLib.ClassHelpers
{
    /// <summary>
    /// Предоставляет общие данные из базы данных
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// Контекст данных
        /// </summary>
        public static ComicLibEntities DBContext { get; set; } = new ComicLibEntities();

        public static BitmapImage Image { get; set; }
        public static User CurrentUser { get; set; }

        public static List<Author> Authors { get; set; }
        public static List<Comic> Comics { get; set; }
        public static List<Genre> Genres { get; set; }
        public static List<Tag> Tags { get; set; }


        /// <summary>
        /// Проверяет наличие учетной записи.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pas"></param>
        /// <param name="message"></param>
        /// <returns>Значение <see langword="true"/>, если учетная запись найдена; в противном случае - значение <see langword="false"/></returns>
        public static bool GetUser(string login, string pas, out string message)
        {
            if (LoginRemember(login, pas))
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
                message = "Пользователь не найден!";
                return false;
            }
        }

        /// <summary>
        /// Проверяет, подходит ли почта по условиям валидации и при неверном формате почты выводит сообщение с информацией об ошибке.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="errorMessage"></param>
        /// <returns>Возвращает <see langword="true"/>, если почта написана верно, в противном случае - <see langword="false"/>.</returns>
        public static bool ValidateEmail(string email, out string errorMessage)
        {
            var input = email;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Все поля должны быть заполнены!";
                return false;
            }

            if (Regex.IsMatch(input, @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?"))
                return true;
            else
            {
                errorMessage = "Неверный формат почты";
                return false;
            }
        }

        /// <summary>
        /// Запоминает пользователя между сеансами приложения для автоматического входа в аккаунт
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pas"></param>
        /// <returns></returns>
        public static bool LoginRemember(string login, string pas)
        {
            CurrentUser = DBContext.User.Where(u => u.Name == login && u.Password == pas).FirstOrDefault();
            if (CurrentUser == null)
                return false;

            Settings.Default.Name = CurrentUser.Name;
            Settings.Default.Password = CurrentUser.Password;
            Settings.Default.LastLoginDate = DateTime.Now;
            Settings.Default.Save();
            return true;
        }

        /// <summary>
        /// Удаляет информацию о последнем авторизованном пользователе
        /// </summary>
        public static void LoginForget()
        {
            CurrentUser = null;
            Settings.Default.Name = null;
            Settings.Default.Password = null;
            Settings.Default.LastLoginDate = DateTime.MinValue;
            Settings.Default.Save();
        }
    }
}
