using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using TopeServer.al.aldi.topeServer.control.executors;
using TopeServer.al.aldi.topeServer.view;
using System.Windows.Forms;

namespace TopeServer
{
    public class OsControlModule : NancyModule
    {
        public const String MODULE_NAME = "OsControlModule";
        public IMessageDeliverer deliverer;
               
        public OsControlModule()
            : base("/os") 
        {
            initCommands();
            initControllers();
        }

        public OsControlModule(IMessageDeliverer del)
            : base("/os")
        {
            this.deliverer = del;

            initCommands();
            initControllers();
        }

        private void initControllers()
        {
            
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
                showMsg("Stand By");
                bool retValue = OsCommandExecutor.standbyPC();
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("{ sucess: " + retValue + " }");
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
                showMsg("Lock Screen");
                bool retValue = true;// OsCommandExecutor.lockScreen();
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("{ sucess: " + retValue+ " }");

            };

            Get["/monitor_on"] = _ => // monitor on
            {
                Console.WriteLine("OsCommandExecutor.turnMonitorOn(true);");
                bool retValue = OsCommandExecutor.turnMonitorOn(true);
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("Monitor On: " + retValue);

            };

            Get["/monitor_off"] = _ => // monitor off
            {
                Console.WriteLine("OsCommandExecutor.turnMonitorOn(false);");
                bool retValue = OsCommandExecutor.turnMonitorOn(false);
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("Monitor Off: " + retValue);

            };

            /* ************ INPUT ************ */

            Get["/input_lock"] = _ => // lock screen
            {
                showMsg("Lock Input");
                bool retValue = OsCommandExecutor.lockInput(true);
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("OsCommandExecutor.lockInput(true): " + retValue);

            };

            Get["/input_unlock"] = _ => // lock screen
            {
                showMsg("Unlock Input");
                bool retValue = OsCommandExecutor.lockInput(false);
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("OsCommandExecutor.lockInput(false): " + retValue);

            };

        }

        public void setDeliverer(IMessageDeliverer d)
        {
            this.deliverer = d;
        }

        public void showMsg(String msg)
        {
            if (null != deliverer)
            {
                deliverer.showMsg(MODULE_NAME, msg);
            }
        }
    }
}
