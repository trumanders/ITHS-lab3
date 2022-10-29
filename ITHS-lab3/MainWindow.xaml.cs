using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
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
        private void btn_Book_Click(object sender, RoutedEventArgs e)
        {
            // Check that everything is entered
            if (UIStuff.CheckIsAllEntered(txb_Name.Text))
            {
                // Convert DatePicker to DateTime
                DateTime selectedDateTime = new DateTime((int)dp_SelectDate.SelectedDate.Value.Year, (int)dp_SelectDate.SelectedDate.Value.Month, (int)dp_SelectDate.SelectedDate.Value.Day);
                bookingSystem.Book(new Booking(selectedDateTime, cbox_SelectTime.Text, cbox_SelectTable.Text, txb_Name.Text));
            }
            UpdateShowBookingsContent();
        }


        // Save-button click
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            bookingSystem.SaveToFile();
        }


        // GenerateBookings - button click
        private void btn_GenerateBookings_Click(object sender, RoutedEventArgs e)
        {
            if (tbx_NumberOfBookingsToGenerate.Text != "")
            {
                bookingSystem.GenerateBookings(Convert.ToInt32(tbx_NumberOfBookingsToGenerate.Text));               
            }
        }

        private void btn_ShowBookings_Click(object sender, RoutedEventArgs e)
        {
            ToggleShowBookings();
        }


        //private void GenerateBookings(object sender, RoutedEventArgs e)
        //{
        //    bookingSystem.GenerateBookings(DataContext, tbx_NumberOfBookingsToGenerate.Value);
        //}


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
                lbx_BookingsOutput.ItemsSource = "";
                showBookings = false;
            }
        }


        private void btn_Unbook_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure?", "Confirm unbooking", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes == mbResult)
            {
                // Put the selected ListBox items (bookings) in a temporary List
                List<string> selectedItems = new List<string>();
                foreach (string item in lbx_BookingsOutput.SelectedItems)
                {
                    selectedItems.Add(item);
                }

                bookingSystem.Unbook(selectedItems);
                UpdateShowBookingsContent();
            }
        }


        private void UpdateShowBookingsContent()
        {
            if (showBookings)
                // Set ListBox content to the List<string> containing string representation of all booking objects

                lbx_BookingsOutput.ItemsSource = bookingSystem.AllBookingsStringList;
        }


        
    }
}
