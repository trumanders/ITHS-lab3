using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ITHS_lab3
{
    public class BookingSystem
    {
        const string CONFIRM_BOOKING_MESSAGE = "Booking confirmed.\nWelcome to the restaurant!";

        // List of all bookings
        public static List<Booking> allBookings = new List<Booking>();


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
            allBookings.Add(newBooking);
            FileHandler.UpdateBookingFile();            
        }


        // Retrun a string containging all bookings
        public static string AllBookingsToString()
        {
            string allBookingString = "";
            allBookings.Select(booking => booking).ToList().ForEach(booking => allBookingString += booking.BookingToString());    
            return allBookingString;
        }


        // Check if the booking is valid / if the table is available at the chosen time
        private bool IsValidBooking(Booking tempBooking)
        {
            bool isValidBooking = true;
            foreach (Booking b in allBookings)            
                if (tempBooking.Date == b.Date && tempBooking.Time == b.Time && tempBooking.Table == b.Table)
                    isValidBooking = false;            
            return isValidBooking;
        }


        // Confirm booking        
        public bool BookingIsConfirmed(Booking b)
        {
            // OBS!!! CALL BOOKING TO STRING IN MESSAGE BOX
            MessageBoxResult mbResult = MessageBox.Show($"Please confirm booking:\n\nDate: {b.Date}\nTime: {b.Time}\nTable: {b.Table}\nName: {b.Name}", $"Confirm booking", MessageBoxButton.OKCancel);
            if (MessageBoxResult.OK == mbResult)
            {
                MessageBox.Show(CONFIRM_BOOKING_MESSAGE);
                return true;
            }
            return false;
        }
    }
}
