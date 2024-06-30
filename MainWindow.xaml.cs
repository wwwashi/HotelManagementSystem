using HotelManagementSystem.Pages;
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

namespace HotelManagementSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrmMain.Navigate(new Autho());
            TimeText();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrmMain.GoBack();
        }
        private void FrmMain_ContentRendered(object sender, EventArgs e)
        {
            if (FrmMain.CanGoBack)
                grid_head.Visibility = Visibility.Visible;
            else
                grid_head.Visibility = Visibility.Hidden;
        }
        private void TimeText()
        {
            time.Content = $"{TimeOfDay()}";
        }
        private string TimeOfDay()
        {
            var currentTime = DateTime.Now;
            if (currentTime.Hour >= 6 && currentTime.Hour <= 12) return "Доброе утро"; //6.00 - 12.00
            if (currentTime.Hour >= 12 && currentTime.Hour <= 18) return "Добрый день"; // 12.00 - 18.00
            if (currentTime.Hour >= 18 && currentTime.Hour <= 21) return "Добрый вечер"; //18.00 - 21.00
            return "Доброй ночи";
        }
    }
}
