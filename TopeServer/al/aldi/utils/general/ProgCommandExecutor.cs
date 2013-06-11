using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.utils.general
{
    public class ProgCommandExecutor
    {
        public static bool openBrowserWithUrl(String url)
        {
            Process process = Process.Start(url);
            try
            {
                Process.GetProcessById(process.Id);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
    }
}
