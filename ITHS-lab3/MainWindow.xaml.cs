using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Linq;


namespace ITHS_lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BookingSystem bookingSystem;
        private bool showBookings;
        private bool workInProgress = false;
        private bool bookingsExist = true;
        private bool isNumberOfBookingsValid;



        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new ViewModel();
            bookingSystem = new BookingSystem();
            tbl_OutputHeader.Text = $"Number of bookings: {bookingSystem.TotNumBookings}";
            btn_GenerateBookings.IsEnabled = false;
            btn_Unbook.IsEnabled = false;
            showBookings = true;
            UpdateShowBookingsContent();
            ButtonControl();
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
                bookingSystem.Book(new Booking(selectedDateTime, cbox_SelectTime.Text, cbox_SelectTable.Text, txb_Name.Text), false);
            }
            UpdateShowBookingsContent();
            if (bookingSystem.AllBookings.Count != 0)
                bookingsExist = true;
            ButtonControl();
        }


        // Save-button click
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            bookingSystem.SaveToFile();
        }


        // Open button-click
        public async void btn_load_Click(object sender, RoutedEventArgs e)
        {
            workInProgress = true;
            ButtonControl();
            
            await Task.Run(() => bookingSystem.ReadFromFile());
            workInProgress = false;            
            lbx_BookingsOutput.ItemsSource = null;
            lbx_BookingsOutput.ItemsSource = bookingSystem.bookingsFromFile;
            bookingSystem.AllBookings.Clear();
            bookingSystem.AllBookingsStringList.Clear();
            bookingsExist = false;
            ButtonControl();
            setInfo($"{bookingSystem.bookingsFromFile.Count} bookings loaded!");
        }


        // Generate bookings with random date, time and table
        private async void btn_GenerateBookings_Click(object sender, RoutedEventArgs e)
        {
            workInProgress = true;
            ButtonControl();
            // Disable save and book-buttons while gernerating
            int numOfBookingsToGenerate;
            if (Int32.TryParse(tbx_NumberOfBookingsToGenerate.Text, out numOfBookingsToGenerate) && numOfBookingsToGenerate > 0)
            {
                await Task.Run(() => bookingSystem.GenerateBookings(numOfBookingsToGenerate));
            }

            else
            {
                MessageBox.Show($"Please enter a number between 1 and {Int32.MaxValue}");
            }
            UpdateShowBookingsContent();
            workInProgress = false;
            bookingsExist = true;
            ButtonControl();
            IsNumberOfBookingsValid();
        }


        private void btn_ShowBookings_Click(object sender, RoutedEventArgs e)
        {
            ToggleShowBookings();
        }


        // Hide or show the bookings list content
        public void ToggleShowBookings()
        {
            if (!showBookings)
            {
                btn_ShowBookings.Content = "Hide bookings";
                showBookings = true;
                UpdateShowBookingsContent();
            }
            else
            {
                btn_ShowBookings.Content = "Show bookings";
                lbx_BookingsOutput.ItemsSource = "";
                showBookings = false;
            }
        }


        // Unbook the user-selected bookings from the list
        private async void btn_Unbook_Click(object sender, RoutedEventArgs e)
        {
            workInProgress = true;
            ButtonControl();

            MessageBoxResult mbResult = MessageBox.Show("Are you sure?", "Confirm unbooking", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes == mbResult)
            {
                // Use array to make unbooking much faster for very large number of unbookings
                int[] selectedIndexes = new int[lbx_BookingsOutput.SelectedItems.Count];

                // UI is not responsive during this operation
                selectedIndexes = (from object item in lbx_BookingsOutput.SelectedItems select lbx_BookingsOutput.Items.IndexOf(item)).ToArray();   
                
                // UI is responsive during unbooking
                await Task.Run(() => bookingSystem.Unbook(selectedIndexes));
                UpdateShowBookingsContent();
            }
            workInProgress = false;
            if (bookingSystem.AllBookings.Count == 0)
                bookingsExist = false;
            ButtonControl();
            //IsNumberOfBookingsValid();
        }


        // Clears all list content and booking objects
        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            lbx_BookingsOutput.ItemsSource = null;            
            bookingSystem.AllBookings.Clear();
            bookingSystem.AllBookingsStringList.Clear();
            bookingsExist = false;
            bookingSystem.ResetNumberOfBookings();
            UpdateShowBookingsContent();
            ButtonControl();
        }



        // Detect when user selects bookings and enable / disable Unbook button
        private void BookingsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbx_BookingsOutput.SelectedItem == null)
                btn_Unbook.IsEnabled = false;
            else
                btn_Unbook.IsEnabled = true;
            if (bookingSystem.AllBookings.Count == 0)
                btn_Unbook.IsEnabled = false;
        }


        // Detect when something is typed in the textbox for number of bookings to generate
        // Disable button if the typed value is invalid
        private void tbx_NumberOfBookingsChanged(object sender, TextChangedEventArgs e)
        {
            ButtonControl();
            //IsNumberOfBookingsValid();                           
        }


        // Checks of the entered number of bookings to generate is valid after each action, and sets the button accordingly
        private void IsNumberOfBookingsValid()
        {
            if (!workInProgress && Int32.TryParse(tbx_NumberOfBookingsToGenerate.Text, out int numOfBookings) && numOfBookings > 0 && numOfBookings <= Int32.MaxValue)
                btn_GenerateBookings.IsEnabled = true;
            else btn_GenerateBookings.IsEnabled = false;
        }


        // Updates the ListBox content
        public void UpdateShowBookingsContent()
        {
            if (showBookings)
            {
                // Set ListBox content to the List<string> containing string representation of all booking objects
                lbx_BookingsOutput.ItemsSource = null;
                lbx_BookingsOutput.ItemsSource = bookingSystem.AllBookingsStringList;
            }
            tbl_OutputHeader.Text = $"Number of bookings: {bookingSystem.TotNumBookings}";
        }


        // Display info about the activity (generating / unbooking / updating list)
        public void setInfo(string infoText)
        {
            tbl_info.Text = infoText;
        }


        // Disable or enable buttons based on activity
        public void ButtonControl()
        {
            if (bookingsExist)
            {
                btn_Book.IsEnabled = true;
                btn_ShowBookings.IsEnabled = true;
                btn_Save.IsEnabled = true;
            }
            else
            {
                btn_Book.IsEnabled = true;
                btn_ShowBookings.IsEnabled = false;
                btn_Save.IsEnabled = false;
            }

            // If generating or unbookings is in progress, all button settings are overridden
            if (workInProgress)
            {
                btn_ShowBookings.IsEnabled = true;
                btn_Book.IsEnabled = false;
                btn_Save.IsEnabled = false;
                btn_GenerateBookings.IsEnabled = false;                
            }
            btn_Unbook.IsEnabled = false;
            IsNumberOfBookingsValid();
        }
    }
}
