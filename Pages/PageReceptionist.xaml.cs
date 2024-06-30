using HotelManagementSystem.Data;
using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
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
    public partial class PageReceptionist : Page
    {
        private Staff currentUser;
        HotelEntityFramework dbContext = new HotelEntityFramework();
        public PageReceptionist(Staff currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            LabelText();

            var rooms = dbContext.Rooms.ToList();
            LViewRooms.ItemsSource = rooms;
            labelEmpty.Visibility = Visibility.Hidden;
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text;

            if (searchText.Length == 0)
            {
                LViewRooms.Visibility = Visibility.Visible;
                labelEmpty.Visibility = Visibility.Hidden;
                var rooms = dbContext.Rooms.ToList();
                LViewRooms.ItemsSource = rooms;
            }
            else
            {
                var filteredAndSortedRooms = GetFilteredAndSortedRooms(searchText, cmbFilter.SelectedIndex, cmbSorting.SelectedIndex);

                if (filteredAndSortedRooms?.Count == 0)
                {
                    LViewRooms.Visibility = Visibility.Hidden;
                    labelEmpty.Visibility = Visibility.Visible;
                }
                else
                {
                    LViewRooms.Visibility = Visibility.Visible;
                    labelEmpty.Visibility = Visibility.Hidden;
                    LViewRooms.ItemsSource = filteredAndSortedRooms;
                }
            }
        }

        private List<Rooms> GetFilteredAndSortedRooms(string searchText, int filterIndex, int sortIndex)
        {
            switch (filterIndex)
            {
                case 0: // номер комнаты
                    return sortIndex == 0
                        ? dbContext.Rooms.Where(u => u.RoomNumber.ToString().Contains(searchText)).OrderBy(u => u.RoomNumber).ToList()
                        : dbContext.Rooms.Where(u => u.RoomNumber.ToString().Contains(searchText)).OrderByDescending(u => u.RoomNumber).ToList();
                case 1: // тип команты
                    return sortIndex == 0
                        ? dbContext.Rooms.Where(u => u.RoomTypes.Name.Contains(searchText)).OrderBy(u => u.RoomTypes.Name).ToList()
                        : dbContext.Rooms.Where(u => u.RoomTypes.Name.Contains(searchText)).OrderByDescending(u => u.RoomTypes.Name).ToList();
                case 2: // цена комнаты
                    return sortIndex == 0
                        ? dbContext.Rooms.Where(u => u.Price.ToString().Contains(searchText)).OrderBy(u => u.Price.ToString()).ToList()
                        : dbContext.Rooms.Where(u => u.Price.ToString().Contains(searchText)).OrderByDescending(u => u.Price.ToString()).ToList();
                default:
                    return null;
            }
        }
        private void LabelText()
        {
            fio.Content = $"{currentUser.FirstName} {currentUser.LastName}";
        }

        private void btnSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // открываем ексель
                Excel.Application excelApp = new Excel.Application();

                // создаем новую книгу
                Excel.Workbook workbook = excelApp.Workbooks.Add();

                // создаем первый лист
                Excel.Sheets sheets = workbook.Sheets;
                Excel.Worksheet sheet = (Excel.Worksheet)sheets.Item[1];

                // название листа
                sheet.Name = "Забронированные комнаты";

                // пишем заголовки 
                sheet.Cells[1, 1] = "ФИО посетителя";
                sheet.Cells[1, 2] = "Номер комнаты";
                sheet.Cells[1, 3] = "Дата заселения";
                sheet.Cells[1, 4] = "Дата выселения";
                sheet.Cells[1, 5] = "Статус";

                var reservations = dbContext.Reservations.ToList();
                // Добавляем пользователей в таблицу
                int row = 2;
                foreach (var reservation in reservations)
                {
                    if (reservation.StatusID == 2) {
                        sheet.Cells[row, 1] = reservation.Guests.FirstName + reservation.Guests.LastName;
                        sheet.Cells[row, 2] = reservation.Rooms.RoomNumber;
                        sheet.Cells[row, 3] = reservation.CheckInDate;
                        sheet.Cells[row, 4] = reservation.CheckOutDate;
                        sheet.Cells[row, 5] = reservation.Statuses.Name;

                        row++;
                    }
                }

                // сохраняем по пути:
                string filePath = @"C:\Users\hwaya\Downloads\meow.xlsx";
                workbook.SaveAs(filePath, Excel.XlFileFormat.xlOpenXMLWorkbook);

                // закрываем
                workbook.Close();
                excelApp.Quit();

                // освобождаем ком объекты
                Marshal.ReleaseComObject(sheet);
                Marshal.ReleaseComObject(sheets);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excelApp);
            }
            catch (Exception ex)
            {
                string message = $"An unexpected error occurred: {ex.Message}\n\n";
                message += $"Stack trace:\n{ex.StackTrace}";
                MessageBox.Show(message);
            }
        }
    }
}
