using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ComicLib
{
    public static class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
           typeof(PasswordHelper));


        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is PasswordBox passwordBox))
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }

        /// <summary>
        /// Определяет, совпадают ли указанные поля пароля и выводит сообщение с информацией об ошибке.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="copyPass"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, string copyPass, out string errorMessage)
        {
            var input = password;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Пароль не может быть пустым";
                return false;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,20}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasMiniMaxChars.IsMatch(input))
            {
                errorMessage = "Пароль не должен быть меньше 8 или больше 20 символов";
                return false;
            }
            else if (!hasLowerChar.IsMatch(input))
            {
                errorMessage = "Пароль должен содержать хотя бы одну строчную букву";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                errorMessage = "Пароль должен содержать хотя бы одну заглавную букву";
                return false;
            }

            else if (!hasNumber.IsMatch(input))
            {
                errorMessage = "Пароль должен содержать хотя бы одно числовое значение";
                return false;
            }
            else if (!hasSymbols.IsMatch(input))
            {
                errorMessage = "Пароль должен содержать хотя бы один символ специального регистра";
                return false;
            }
            else if (password != copyPass)
            {
                errorMessage = "Пароли не совпадают";
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Проверяет, подходит ли пароль по условиям валидации и при неверном пароле выводит сообщение с информацией об ошибке.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="errorMessage"></param>
        /// <returns>Возвращает <see langword="false"/>, если пароль написан верно, в противном случае - <see langword="true"/>.</returns>
        public static bool ValidatePassword(string password, out string errorMessage)
        {
            var input = password;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Все поля должны быть заполнены!";
                return false;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,20}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasMiniMaxChars.IsMatch(input))
            {
                errorMessage = "Пароль не должен быть меньше 8 или больше 20 символов";
                return false;
            }
            else if (!hasLowerChar.IsMatch(input))
            {
                errorMessage = "Пароль должен содержать хотя бы одну строчную букву";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                errorMessage = "Пароль должен содержать хотя бы одну заглавную букву";
                return false;
            }

            else if (!hasNumber.IsMatch(input))
            {
                errorMessage = "Пароль должен содержать хотя бы одно числовое значение";
                return false;
            }
            else if (!hasSymbols.IsMatch(input))
            {
                errorMessage = "Пароль должен содержать хотя бы один символ специального регистра";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
