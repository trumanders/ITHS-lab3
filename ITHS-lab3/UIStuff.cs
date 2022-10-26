using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ITHS_lab3
{
    public class UIStuff
    {
        public static bool isDateSelected = false;
        public static bool isTableSelected = false;
        public static bool isTimeSelected = false;
        public static bool showBookings = false;

        public static bool CheckIsAllEntered(string enteredName)
        {
            if (!isDateSelected)
            {
                MessageBox.Show("Please select a date");
                return false;
            }
            else if (!isTimeSelected)
            {
                MessageBox.Show("Please select a time");
                return false;
            }
            else if (!isTableSelected)
            {
                MessageBox.Show("Please select a table");
                return false;
            }
            else if (enteredName == "Booker's name" || enteredName == null)
            {
                MessageBox.Show("Please enter a name");
                return false;
            }
            return true;
        }



    }
}
