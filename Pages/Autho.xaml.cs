using HotelManagementSystem.Data;
using Microsoft.Win32;
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

namespace HotelManagementSystem.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        int countUnsuccessful = 0;
        public Autho()
        {
            InitializeComponent();
            PanelCapcha.Visibility = Visibility.Hidden;
            pbPassword.PasswordChar = '*';
            tbPassword.Visibility = Visibility.Hidden;
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbLogin.Text) && !String.IsNullOrEmpty(pbPassword.Password))
                LoginUser();
            else
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль", "Предупреждение");
                GenerateCaptcha();
                if (countUnsuccessful % 1 == 0)
                {
                    TimerBLock();
                    return;
                }
            }
        }
        private void cbShowPassword_Clicked(object sender, RoutedEventArgs e)
        {
            if (cbShowPassword.IsChecked == true)
            {
                tbPassword.Text = pbPassword.Password; // Показать пароль
                tbPassword.Visibility = Visibility.Visible;
                pbPassword.Visibility = Visibility.Hidden;
            }
            else
            {
                pbPassword.Password = tbPassword.Text; // Скрыть пароль
                tbPassword.Visibility = Visibility.Hidden;
                pbPassword.Visibility = Visibility.Visible;
            }
        }

        private void LoginUser()
        {
            if (countUnsuccessful > 0)
            {
                if (!CheckingCaptcha())
                {
                    MessageBox.Show("Неверная капча", "Предупреждение");
                    countUnsuccessful++;
                    TimerBLock();
                    GenerateCaptcha();
                    return;
                }
            }

            HotelEntityFramework dbContext = new HotelEntityFramework();
            Staff user = new Staff();

            string Username = tbLogin.Text.Trim();
            string Password = pbPassword.Password.Trim();

            user = dbContext.Staff.Where(p => p.Username == Username).FirstOrDefault();
            if (user != null)
            {
                if (user?.Password == Password)
                {
                    LoadForm(user.PositionID.ToString(), user);
                    countUnsuccessful = 0;
                    tbLogin.Text = "";
                    pbPassword.Password = "";
                    tbPassword.Text = "";
                    tboxCaptcha.Text = "";
                    PanelCapcha.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Неверный пароль", "Предупреждение");
                    GenerateCaptcha();
                    countUnsuccessful++;
                    if (countUnsuccessful == 1)
                        TimerBLock();
                }
            }
            else
            {
                MessageBox.Show("Пользователя с именем '" + Username + "' не существует", "Предупреждение");
                GenerateCaptcha();
                countUnsuccessful++;
                if (countUnsuccessful == 1)
                    TimerBLock();
                return;
            }
        }
        private async void TimerBLock()
        {
            panelLogin.IsEnabled = false;

            await Task.Factory.StartNew(() =>
            {
                for (int i = 10; i > 0; i--)
                {
                    //Каждую секунду вызывает метод для обновления текста
                    tblockTimer.Dispatcher.Invoke(() =>
                    {
                        tblockTimer.Text = $"подождите {i} сек";
                    });
                    Task.Delay(1000).Wait();//Приостанавливает выполнение задачи на 1 секунду
                }
            });

            tblockTimer.Text = "";
            panelLogin.IsEnabled = true;
        }
        private void GenerateCaptcha()
        {
            PanelCapcha.Visibility = Visibility.Visible;

            char[] letters = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            Random rand = new Random();

            string word = "";
            for (int j = 1; j <= 6; j++)
            {
                int letter_num = rand.Next(0, letters.Length - 1);
                word += letters[letter_num];
            }

            tblockCaptcha.Text = word;
            tblockCaptcha.TextDecorations = TextDecorations.Strikethrough;
            tboxCaptcha.Text = "";
        }
        private bool CheckingCaptcha() => tblockCaptcha.Text == tboxCaptcha.Text.Trim();
        private void LoadForm(string _role, Staff user)
        {
            switch (_role)
            {
                //Manager
                case "1":
                    NavigationService.Navigate(new PageManager(user));
                    break;
                //Receptionist 
                case "2":
                    NavigationService.Navigate(new PageReceptionist(user));
                    break;
                //Waiter
                case "3":
                    NavigationService.Navigate(new PageWaiter(user));
                    break;
            }
        }
    }
}
