using Microsoft.VisualBasic;
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
        private bool showBookings = false;
        BookingSystem bookingSystem;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
            bookingSystem = new BookingSystem();
            FileHandler.CreateFile();
        }


        // DATE SELECTION
        private void dp_SelectDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UIStuff.isDateSelected = true;
        }

        // TABLE SELECTION
        private void cbox_SelectTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UIStuff.isTableSelected = true;
        }


        // TIME SELECTION
        private void cbox_SelectTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UIStuff.isTimeSelected = true;
        }


        // Book - BUTTON CLICKED
        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            // Check that everything is entered
            if (UIStuff.CheckIsAllEntered(txb_Name.Text))
            {
                // Pass entered info directly
                bookingSystem.Book(new Booking(dp_SelectDate, cbox_SelectTime.Text, cbox_SelectTable.Text, txb_Name.Text));
            }           
            UpdateShowBookingsContent();
        }


        private void btn_ShowBookings_Click(object sender, RoutedEventArgs e)
        {
            ToggleShowBookings();
        }

        public void ToggleShowBookings()
        {
            if (!showBookings)
            {
                tbl_OutputHeader.Text = "Bookings:";
                btn_ShowBookings.Content = "Hide bookings";
                showBookings = true;
                UpdateShowBookingsContent();
            }
            else
            {
                tbl_OutputHeader.Text = "";
                btn_ShowBookings.Content = "Show bookings";
                tbo_BookingsOutput.Text = "";
                showBookings = false;
            }
        }

        private void UpdateShowBookingsContent()
        {
            if (showBookings)
                tbo_BookingsOutput.Text = BookingSystem.AllBookingsToString();
        }
    }
}
