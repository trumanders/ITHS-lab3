using System;
using System.Collections.Generic;
using System.Text;

namespace ITHS_lab3
{
    class Booking
    {
        public string DateStrFormat { get; set; }
        public string Time { get; set; }
        public string Table { get; set; }
        public string Name { get; set; }

        public Booking(string dateStrFormat, string time, string table, string name)
        {
            DateStrFormat = dateStrFormat;
            Time = time;
            Table = table;
            Name = name;
        }


        // Get booking string (YYYY-MM-DD: Name, table num - Time)
        public string BookingToString()
        {
            return $"{DateStrFormat}: {Name}, table {Table} - {Time}\n";
        }

        public void ShowBookingConfirmation()
        {

        }


        public void ConfirmBooking()
        {

        }



 
    }
}
