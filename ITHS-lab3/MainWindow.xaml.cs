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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ITHS_lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedDateStrFormat;
        private string selectedTable;
        private string selectedTime;
        private string selectedName;

        bool isDateSelected = false;
        bool isTableSelected = false;
        bool isTimeSelected = false;
        bool isShowingBookings = false;

        public MainWindow()
        {
            InitializeComponent();
        }


        // DATE SELECTION
        private void dp_SelectDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDateStrFormat = $"{dp_SelectDate.SelectedDate.Value.Year.ToString()}-{dp_SelectDate.SelectedDate.Value.Month.ToString()}-{dp_SelectDate.SelectedDate.Value.Day.ToString()}";
            isDateSelected = true;
        }

        // TABLE SELECTION
        private void cbox_SelectTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTable = cbox_SelectTable.SelectedValue.ToString();
            isTableSelected = true;
        }


        // TIME SELECTION
        private void cbox_SelectTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTime = cbox_SelectTime.SelectedValue.ToString();
            isTimeSelected = true;
        }


        // OK BUTTON CLICKED
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            if (!isDateSelected) MessageBox.Show("Please select a date");
            else if (!isTimeSelected) MessageBox.Show("Please select a time");
            else if (!isTableSelected) MessageBox.Show("Please select a table");
            else if (txb_name.Text == "Booker's name" || txb_name.Text == null) MessageBox.Show("Please enter a name");

            if (isDateSelected && isTimeSelected && isTableSelected)
            {
                if (BookingIsConfirmed())
                    BookingSystem.MakeBooking(selectedDateStrFormat, selectedTime, selectedTable, txb_name.Text);
            }
        }


        // Confirm booking
        private bool BookingIsConfirmed()
        {
            MessageBoxResult mbResult = MessageBox.Show($"Please confirm booking:\n\nDate: {selectedDateStrFormat}\nTime: {selectedTime}\nTable: {selectedTable}\nName: {txb_name.Text}", $"Confirm booking", MessageBoxButton.OKCancel);
            if (MessageBoxResult.OK == mbResult)
            {
                MessageBox.Show("Booking confirmed.");
                return true;
            }
            return false;
        }

        private void btn_ShowBookings_Click(object sender, RoutedEventArgs e)
        {
            if (!isShowingBookings)
            {
                txb_OutputHeader.Text = "Bookings:";
                txb_BookingsOutput.Text = BookingSystem.AllBookingsToString();
                isShowingBookings = true;
                btn_ShowBookings.Content = "Hide bookings";
            }
            else
            {
                txb_OutputHeader.Text = "";
                txb_BookingsOutput.Text = "";
                isShowingBookings = false;
                btn_ShowBookings.Content = "Show bookings";
            }

            
        }


        // DISPLAY SELECTION
        private void DisplaySelections()
        {
            if (isDateSelected == true && isTableSelected == true && isTimeSelected == true)
                txb_BookingsOutput.Text = $"Date: {selectedDateStrFormat}\nTime: {selectedTime}\nTable: {selectedTable}";
        }
    }
}
