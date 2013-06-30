using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.utils.general
{
    public class TopeLogger
    {
        public static void Log(String lines)
        {

            // Write the string to a file.append mode is enabled so that the log
            // lines get appended to  test.txt than wiping content and writing the log

            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("topeLog.txt", true);
                file.WriteLine("");
                file.WriteLine(lines);

                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);// Well you cant log the log!
            }

        }
    }
}
