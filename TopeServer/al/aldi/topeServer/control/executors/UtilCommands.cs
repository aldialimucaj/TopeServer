using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.utils.general;

namespace TopeServer.al.aldi.topeServer.control.executors
{
    public class UtilCommands
    {
        public static bool showMsg(TopeRequest request)
        {
            return UtilsCommandExecutor.showMsg(request.arg0);
        }

        public static bool beep(TopeRequest request)
        {
            return UtilsCommandExecutor.beep(request.arg0);
        }

        public static bool readOutLoud(TopeRequest request)
        {
            return UtilsCommandExecutor.readOutLoud(request.arg0);
        }
    }
}
