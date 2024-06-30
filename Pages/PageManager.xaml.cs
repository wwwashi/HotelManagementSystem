using HotelManagementSystem.Data;
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
    /// Логика взаимодействия для PageManager.xaml
    /// </summary>
    public partial class PageManager : Page
    {
        private Staff currentUser;
        public PageManager(Staff currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            LabelText();
        }
        private void LabelText()
        {
            fio.Content = $"{currentUser.FirstName} {currentUser.LastName}";
        }
        
    }
}
