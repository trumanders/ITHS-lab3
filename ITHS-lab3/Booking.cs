using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ITHS_lab3
{
    public class Booking
    {
        public DateTime Date{ get; set; }
        public string Time { get; set; }
        public string Table { get; set; }
        public string Name { get; set; }

        public Booking(DateTime date, string time, string table, string name)
        {
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
