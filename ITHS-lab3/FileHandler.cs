using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ITHS_lab3
{
    public class FileHandler
    {
        private const string FILENAME = "bookings.txt";

        private BookingSystem bookingSystem;


        public FileHandler(BookingSystem bookingSystem)
        {
            this.bookingSystem = bookingSystem;
        }


       public static void CreateFile()
        {
            if (!File.Exists(FILENAME))
                File.AppendAllText(FILENAME, "");
        }



        public static void UpdateBookingFile()
        {
            File.WriteAllLines(FILENAME, BookingSystem.AllBookingsToStringArr());
        }
    }
}
