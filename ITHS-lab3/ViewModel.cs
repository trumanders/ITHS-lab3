using System;
using System.Collections.Generic;
using System.Text;

namespace ITHS_lab3
{
    public class ViewModel
    {
        public List<string> comboBoxTables { get; private set; }
        public List<string> comboBoxTimes { get; private set; }

        public ViewModel()
        {
            comboBoxTables = new List<string>() { "1", "2", "3", "4", "5" };
            comboBoxTimes = new List<string> { "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00" };
        }
    }
}
