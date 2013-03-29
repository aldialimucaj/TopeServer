using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using TopeServer.al.aldi.topeServer.control.executors;

namespace TopeServer
{
    public class OsControlModule : NancyModule
    {
        
        public OsControlModule()
            : base("/os") 
        {
            initCommands();
        }

        private void initCommands()
        {
            Get["/"] = _ => "Please specify a command!"; // default route

            Get["/hibernate"] = _ => // hibernating pc
            {
                bool retValue = OsCommandExecutor.hibernatePC();
                return "PC hibernated: " + retValue;
            };

            Get["/standby"] = _ => // standby
            {
                bool retValue = OsCommandExecutor.standbyPC();
                return "PC standby: " + retValue;
            };

            Get["/poweroff"] = _ => // suspend pc
            {
                bool retValue = OsCommandExecutor.powerOffPC();
                return "PC poweroff: " + retValue;
            };

            Get["/logoff"] = _ => // logoff pc
            {
                bool retValue = OsCommandExecutor.logOffPC();
                return "PC logoff: " + retValue;
            };

            Get["/lock_screen"] = _ => // lock screen
            {
                bool retValue = OsCommandExecutor.lockScreen();
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("Lock Screen: " + retValue);

            };

        }
    }
}
