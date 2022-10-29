using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Collections.ObjectModel;

namespace ITHS_lab3
{
    public class BookingSystem
    {
        const string CONFIRM_BOOKING_MESSAGE = "Booking confirmed.\nWelcome to the restaurant!";
        const string FILENAME = "bookings.txt";

        // List of all bookings
        public List<Booking> AllBookings { get; set; }
        public List<string> AllBookingsStringList { get; set; }
        public List<string> comboBoxTables { get; private set; }
        public List<string> comboBoxTimes { get; private set; }      

        public BookingSystem()
        {
            AllBookings = new List<Booking>();
            AllBookingsStringList = new List<string>();
            if (!File.Exists(FILENAME))
                File.AppendAllText(FILENAME, "");
            comboBoxTables = new List<string>() { "1", "2", "3", "4", "5" };
            comboBoxTimes = new List<string> { "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00" };
        }


        // Auto-generate bookings
        public void GenerateBookings(int numberOfBookingsToGenereate)
        {
            Random rnd = new Random();            
            for (int i = 0; i < numberOfBookingsToGenereate; i++)
            {
                // Generate random month and day
                int randomMonth = rnd.Next(11) + 1;
                int randomDay = rnd.Next(27) + 1;

                // Generate random indexes for the time- and table selections arrays
                string randomTime = comboBoxTimes[rnd.Next(comboBoxTimes.Count)];
                string randomTable = comboBoxTables[rnd.Next(comboBoxTables.Count)];
                AllBookings.Add(new Booking(new DateTime(2023, randomMonth, randomDay), randomTime, randomTable, "")); 
            }
            
            UpdateBookingsStringList();
        }


        // Calls method to make the booking if it is valid
        public void Book(Booking tempBooking)
        {
            if (IsValidBooking(tempBooking))
            {
                if (BookingIsConfirmed(tempBooking))
                    MakeBooking(tempBooking);
            }
            else MessageBox.Show("Already booked! Please try another table, time or day.");
        }


        // Creates a booking object in the AllBookings List
        public void MakeBooking(Booking newBooking)
        {
            AllBookings.Add(newBooking);
            UpdateBookingsStringList();
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
        public bool BookingIsConfirmed(Booking b)
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
        public void Unbook(List<string> selectedItems)
        {
            /* Since the ListBox content is not objects, the bookings are compared
             as strings. */
            // For each booking in selected bookings List...
            foreach (string selectedBooking in selectedItems)
            {
                int index = 0;
                // Check if it is the same as a booking in AllBookingsStringList
                foreach (string booking in AllBookingsStringList)
                {
                    if (AllBookingsStringList[index] == selectedBooking)
                    {
                        // Remove the booking in the string list, and in the object list
                        AllBookingsStringList.RemoveAt(index);
                        AllBookings.RemoveAt(index);
                        break;
                    }
                    index++;
                }
            }
            UpdateBookingsStringList();
        }


        // Update string List of bookings
        public void UpdateBookingsStringList()
        {
            // Call booking to string on each booking and put it in a string array
            AllBookingsStringList = AllBookings.Select(booking => booking.BookingToString()).ToList();
        }


        // Save bookings to txt-file
        public void SaveToFile()
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                string filename = sfd.FileName;
                sfd.DefaultExt = ".txt";
                // Write to file
                File.WriteAllLines(filename, AllBookingsStringList);
            }
        }
    }
}
