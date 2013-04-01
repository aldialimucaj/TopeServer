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

            /* ************ POWER ************ */

            Get["/hibernate"] = _ => // hibernating pc
            {
                Console.WriteLine("OsCommandExecutor.hibernatePC();");
                bool retValue = true;// OsCommandExecutor.hibernatePC();
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

            Get["/restart"] = _ => // restart pc
            {
                bool retValue = OsCommandExecutor.restartPC();
                return "PC restart: " + retValue;
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

            /* ************ INPUT ************ */

            Get["/input_lock"] = _ => // lock screen
            {
                Console.WriteLine("OsCommandExecutor.lockInput(true);");
                bool retValue = OsCommandExecutor.lockInput(true);
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("OsCommandExecutor.lockInput(true): " + retValue);

            };

            Get["/input_unlock"] = _ => // lock screen
            {
                Console.WriteLine("OsCommandExecutor.lockInput(false);");
                bool retValue = OsCommandExecutor.lockInput(false);
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("OsCommandExecutor.lockInput(false): " + retValue);

            };

        }
    }
}
