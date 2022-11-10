using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ITHS_lab3
{
    public class BookingSystem
    {
        const string CONFIRM_BOOKING_MESSAGE = "Booking confirmed.\nWelcome to the restaurant!";
        const string FILENAME = "bookings.txt";

        // List of all bookings
        public List<Booking> AllBookings { get; set; }
        public List<string> AllBookingsStringList { get; set; }
        ViewModel viewModel;
        public List<string> bookingsFromFile { get; private set; }

        public int TotNumBookings
        {
            get { return AllBookings.Count; }
            private set { }
        }

        public BookingSystem()
        {
            viewModel = new ViewModel();
            AllBookings = new List<Booking>();
            AllBookingsStringList = new List<string>();
            if (!File.Exists(FILENAME))
                File.AppendAllText(FILENAME, "");
        }


        // Auto-generate bookings
        public void GenerateBookings(int numOfBookingsToGenerate)
        {
            Random rnd = new Random();

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.setInfo("Generating...");
            });

            for (int i = 0; i < numOfBookingsToGenerate; i++)
            {
                // Set random month and day
                int randomMonth = rnd.Next(11) + 1;
                int randomDay = rnd.Next(27) + 1;

                // Generate random indexes in the time- and table arrays
                string randomTime = viewModel.comboBoxTimes[rnd.Next(viewModel.comboBoxTimes.Count)];
                string randomTable = viewModel.comboBoxTables[rnd.Next(viewModel.comboBoxTables.Count)];

                // Add booking object directly (no valid checking)
                AllBookings.Add(new Booking(new DateTime(2023, randomMonth, randomDay), randomTime, randomTable, ""));
            }

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.setInfo($"{numOfBookingsToGenerate} bookings gererated!");
            });
            UpdateBookingsStringList();
        }



        // Perform booking if it is valid
        public void Book(Booking tempBooking)
        {
            if (IsValidBooking(tempBooking))
            {
                if (BookingIsConfirmed(tempBooking))
                    MakeBooking(tempBooking);
            }
            else MessageBox.Show("Already booked! Please try another table, time or day.");
            UpdateBookingsStringList();
        }


        // Creates a booking object in the AllBookings List
        private void MakeBooking(Booking newBooking)
        {
            AllBookings.Add(newBooking);
        }


        // Check if the booking is valid / if the table is available at the chosen time
        private bool IsValidBooking(Booking tempBooking)
        {
            bool isValidBooking = true;
            foreach (Booking b in AllBookings)
                if (tempBooking.Date == b.Date && tempBooking.Time == b.Time && tempBooking.Table == b.Table)
                    isValidBooking = false;
            return isValidBooking;
        }


        // Confirm booking        
        private bool BookingIsConfirmed(Booking b)
        {
            MessageBoxResult mbResult = MessageBox.Show($"Please confirm booking:\n\nDate: {b.Date.ToString("yyyy/MM/dd")}\nTime: {b.Time}\nTable: {b.Table}\nName: {b.Name}", $"Confirm booking", MessageBoxButton.OKCancel);
            if (MessageBoxResult.OK == mbResult)
            {
                MessageBox.Show(CONFIRM_BOOKING_MESSAGE);
                return true;
            }
            return false;
        }


        // Delete selected bookings in the ListBox
        public void Unbook(int[] selectedIndexes)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.setInfo($"Unbooking...");                
            });

            Array.Sort(selectedIndexes);
            for (int i = selectedIndexes.Length - 1; i >= 0; i--)
            {
                //MessageBox.Show("" + selectedIndexes[i]);
                AllBookings.RemoveAt(selectedIndexes[i]);
                AllBookingsStringList.RemoveAt(selectedIndexes[i]);
                //MessageBox.Show("" + AllBookingsStringList.Count);
            }

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.setInfo($"{selectedIndexes.Length} bookings removed!");
            });
        }


        // Update string List of bookings
        private void UpdateBookingsStringList()
        {

            // Set info-textblock
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.setInfo("Updating list...");
            });

            // Call booking to string on each booking and put it in a string array
            AllBookingsStringList = AllBookings.Select(booking => booking.BookingToString()).ToList();

            // Empty info-textblock
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.setInfo("");
            });
        }


        // Save bookings to txt-file
        public void SaveToFile()
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();

            sfd.DefaultExt = ".txt";
            sfd.Filter = "*.txt | *.TXT";
            if (sfd.ShowDialog() == true)
            {
                string filename = sfd.FileName;

                // Write to file
                try
                {
                    File.WriteAllLines(filename, AllBookingsStringList);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }


        public void ReadFromFile()
        {            
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "*.txt | *.TXT";
            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                // Open file
                try
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MainWindow window = (MainWindow)Application.Current.MainWindow;
                        window.setInfo("Reading from file...");
                    });                    
                    bookingsFromFile = File.ReadAllLines(filename).ToList();
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }            
        }
    }
}
