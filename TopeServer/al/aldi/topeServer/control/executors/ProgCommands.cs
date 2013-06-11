using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.utils.general;

namespace TopeServer.al.aldi.topeServer.control.executors
{
    public class ProgCommands
    {
        public static bool openBrowserWithUrl(TopeRequest request)
        {
            return ProgCommandExecutor.openBrowserWithUrl(request.arg0);
        }
    }
}
