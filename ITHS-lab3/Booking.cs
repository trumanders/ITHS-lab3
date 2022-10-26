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

        public Booking(DatePicker date, string time, string table, string name)
        {
            // Convert datepicker object to DateTime here
            this.Date = new DateTime(date.SelectedDate.Value.Year, date.SelectedDate.Value.Month, date.SelectedDate.Value.Day);
            Time = time;
            Table = table;
            Name = name;
        }


        // Get booking string (YYYY-MM-DD: Name, table num - Time)
        public string BookingToString()
        {
            // Convert DateTime to string date here



            return $"{Date.ToString()}: {Name}, table {Table} - {Time}\n";
        } 
    }
}
