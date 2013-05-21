using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.utils.general
{
    class SystemUtils
    {
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            long returnVal = (long)(DateTime.Now - Jan1st1970).TotalMilliseconds;
            return returnVal;
        }
    }
}
