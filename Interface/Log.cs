using System;

namespace Interface
{
    /// <summary>
    /// Made by chaogeb
    /// </summary>
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
