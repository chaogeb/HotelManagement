using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interface;

namespace Interface
{
    public class Log
    {
        public DateTime Time    { get; set; }
        public string LogText   { get; set; }

        public Log(string logtext)
        {
            LogText = logtext;
            Time = IClock.Time;
        }
        public Log(DateTime time, string logtext)
        {
            LogText = logtext;
            Time = time;
        }
    }
}
