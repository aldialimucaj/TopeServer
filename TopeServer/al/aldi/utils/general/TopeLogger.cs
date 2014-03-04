using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.utils.general
{
    public class TopeLogger
    {
        public static void Log(String lines)
        {
            EventLog.WriteEntry(Program.LOG_TAG, lines);
        }

        public static void Error(String lines)
        {
            EventLog.WriteEntry(Program.LOG_TAG, lines, EventLogEntryType.Error);
        }
    }
}
