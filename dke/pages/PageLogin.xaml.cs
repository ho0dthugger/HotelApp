using dke.file;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace dke.pages
{
    public partial class PageLogin : Page
    {
        private prakDKEEntities dbContext;

        public PageLogin()
        {
            InitializeComponent();
            dbContext = new prakDKEEntities();

            // Подписываемся на событие Click для кнопки "Закрыть приложение"
            btnClose.Click += btnClose_Click;

            // Подписываемся на событие Click для кнопки "Свернуть окно"
            btnMinimize.Click += btnMinimize_Click;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (IsValidLogin(username, password, out string userRole))
            {
                txtStatus.Text = "Успешная авторизация!";
                txtStatus.Foreground = System.Windows.Media.Brushes.Green;

                if (userRole == "Администратор")
                {
                    NavigationService.Navigate(new PageAdmin());
                }
                else if (userRole == "Руководитель")
                {
                    NavigationService.Navigate(new PageManager());
                }
                else if (userRole == "Сотрудник")
                {
                    NavigationService.Navigate(new PageMain());
                }
            }
            else
            {
                var user = dbContext.Users.FirstOrDefault(u => u.ULogin == username);
                if (user != null)
                {
                    // Проверяем, заблокирован ли пользователь
                    if ((bool)user.Blocked)
                    {
                        txtStatus.Text = "Ваша учетная запись заблокирована!";
                        txtStatus.Foreground = System.Windows.Media.Brushes.Red;
                        return;
                    }

                    if (user.LastSuccessfulLogin != null && user.LastSuccessfulLogin.Value.AddDays(30) < DateTime.Now)
                    {
                        txtStatus.Text = "Ваш аккаунт неактивен более 30 дней. Обратитесь к администратору.";
                        txtStatus.Foreground = System.Windows.Media.Brushes.Red;
                        return;
                    }

                    // Проверяем количество неудачных попыток
                    if (user.LastLoginAttempt != null && user.LastLoginAttempt.Value.AddMinutes(30) > DateTime.Now)
                    {
                        if (user.LoginAttempts >= 3)
                        {
                            user.Blocked = true;
                            dbContext.SaveChanges();
                            txtStatus.Text = "Ваша учетная запись заблокирована!";
                        }
                        else
                        {
                            txtStatus.Text = $"Неверный логин или пароль! Осталось {3 - user.LoginAttempts} попыток";
                            txtStatus.Foreground = System.Windows.Media.Brushes.Red;
                        }
                    }
                    else
                    {
                        // Сбрасываем счетчик неудачных попыток, если прошло более 30 минут
                        user.LoginAttempts = 0;
                        user.LastLoginAttempt = DateTime.Now;
                        dbContext.SaveChanges();

                        txtStatus.Text = $"Неверный логин или пароль! Осталось {3 - user.LoginAttempts} попыток";
                        txtStatus.Foreground = System.Windows.Media.Brushes.Red;
                    }
                }
                else
                {
                    txtStatus.Text = "Пользователь не найден!";
                    txtStatus.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
        }

        private bool IsValidLogin(string username, string password, out string userRole)
        {
            // Ищем пользователя в базе данных
            var user = dbContext.Users.FirstOrDefault(u => u.ULogin == username);

            if (user != null)
            {
                // Проверяем, заблокирован ли пользователь
                if ((bool)user.Blocked)
                {
                    userRole = null;
                    return false;
                }

                if (user.LastSuccessfulLogin != null && user.LastSuccessfulLogin.Value.AddDays(30) < DateTime.Now)
                {
                    userRole = null;
                    return false;
                }

                // Проверяем пароль
                if (user.UPassword == password)
                {
                    // Сбрасываем счетчик неудачных попыток
                    user.LoginAttempts = 0;
                    // Обновляем время последнего успешного входа
                    user.LastSuccessfulLogin = DateTime.Now;
                    dbContext.SaveChanges();

                    userRole = user.Roles.RName;
                    return true;
                }
                else
                {
                    // Увеличиваем счетчик неудачных попыток
                    user.LoginAttempts += 1;
                    user.LastLoginAttempt = DateTime.Now;
                    dbContext.SaveChanges();
                }
            }

            userRole = null;
            return false;
        }
    }
}
