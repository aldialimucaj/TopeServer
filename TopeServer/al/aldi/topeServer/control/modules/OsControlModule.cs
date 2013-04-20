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
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.topeServer.control;
using Nancy.TinyIoc;

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
            var container = TinyIoCContainer.Current;
            IMessageDeliverer mdl = container.Resolve<IMessageDeliverer>();
            setDeliverer(mdl);
        }

        private void initControllers()
        {
            
        }

        private void initCommands()
        {
            Get["/"] = _ => "TopeServer running..."; // default route

            /* ************ POWER ************ */

            Get["/hibernate"] = _ => // hibernating pc
            {
                showMsg("Hibernate");
                Console.WriteLine("OsCommandExecutor.hibernatePC();");
                bool retValue = OsCommandExecutor.hibernatePC();
                return "PC hibernated: " + retValue;
            };

            Get["/standby"] = _ => // standby
            {
                showMsg("Stand By");
                bool retValue = OsCommandExecutor.standbyPC();
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("{ success: " + retValue + " }");
            };

            Get["/poweroff"] = _ => // suspend pc
            {
                showMsg("Power Off");
                TopeResponse topeRes = new TopeResponse(true);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);

                bool retValue = OsCommandExecutor.powerOffPC();
                return nego.Response;
            };

            Get["/restart"] = _ => // restart pc
            {
                showMsg("Restart");
                bool retValue = OsCommandExecutor.restartPC();
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("{ \"success\": " + retValue + " }");
            };

            Get["/logoff"] = _ => // logoff pc
            {
                showMsg("Log off");
                bool retValue = OsCommandExecutor.logOffPC();
                return "PC logoff: " + retValue;
            };

            Get["/lock_screen"] = _ => // lock screen
            {
                showMsg("Lock Screen");
                bool retValue = OsCommandExecutor.lockScreen();
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
