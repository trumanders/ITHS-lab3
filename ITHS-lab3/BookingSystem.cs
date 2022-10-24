using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;

namespace ITHS_lab3
{
    static class BookingSystem
    {
        // List of all bookings
        public static List<Booking> AllBookings = new List<Booking>();


        // Save the booking into the list
        public static void MakeBooking(string date, string time, string table, string name)
        {
            AllBookings.Add(new Booking(date, time, table, name));
        }

        // Retrun booking info string for any table
        public static string ShowTableBooking(string tableNumber)
        {
            var bookingForTable = (Booking)AllBookings.Where(booking => booking.Table == tableNumber).Select(booking => booking);
            return bookingForTable.BookingToString();
        }

        // Retrun a string containging all bookings
        public static string AllBookingsToString()
        {
            string allBookingString = "";
            AllBookings.Select(booking => booking).ToList().ForEach(booking => allBookingString += booking.BookingToString());    
            return allBookingString;
        }
    }
}
