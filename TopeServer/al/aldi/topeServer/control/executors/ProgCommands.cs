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
        public static TopeResponse openBrowserWithUrl(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = ProgCommandExecutor.openBrowserWithUrl(request.arg0);
            return topeResponse;
        }

        public static TopeResponse appControlPowerPoint(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = ProgCommandExecutor.appInputSimulation(request.arg0);
            return topeResponse;
        }

        public static TopeResponse appControlVLC(TopeRequest request)
        {
            TopeResponse topeResponse = new TopeResponse();
            topeResponse.success = ProgCommandExecutor.appInputSimulation(request.arg0);
            return topeResponse;
        }
    }
}
