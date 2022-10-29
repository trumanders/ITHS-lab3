using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ITHS_lab3
{
    public class Booking
    {
        public DateTime Date{ get; private set; }
        public string Time { get; private set; }
        public string Table { get; private set; }
        public string Name { get; private set; }

        public Booking(DateTime date, string time, string table, string name)
        {
            // Convert datepicker object to DateTime here
            this.Date = date;
            Time = time;
            Table = table;
            Name = name;           
        }


        // Get booking string
        public string BookingToString()
        {
            return $"{Date.ToString("yyy/MM/dd")}: {Name}, table {Table} - {Time}";
        } 
    }
}
