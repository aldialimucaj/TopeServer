using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;
using TopeServer.al.aldi.topeServer.control.executors;
using TopeServer.al.aldi.topeServer.view;
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.topeServer.control;
using TopeServer.al.aldi.topeServer.control.modules;
using TopeServer.al.aldi.utils.security;

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
                bool retValue = OsCommandExecutor.hibernatePC();
                TopeResponse topeRes = new TopeResponse(retValue);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/standby"] = _ => // standby
            {
                showMsg("Stand By");
                bool retValue = OsCommandExecutor.standbyPC();
                TopeResponse topeRes = new TopeResponse(retValue);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/poweroff"] = _ => // suspend pc
            {
                showMsg("Power Off");
                bool retValue = OsCommandExecutor.powerOffPC();
                TopeResponse topeRes = new TopeResponse(retValue);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/restart"] = _ => // restart pc
            {
                showMsg("Restart");
                bool retValue = OsCommandExecutor.restartPC();
                TopeResponse topeRes = new TopeResponse(retValue);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/logoff"] = _ => // logoff pc
            {
                showMsg("Log off");
                bool retValue = OsCommandExecutor.logOffPC();
                TopeResponse topeRes = new TopeResponse(retValue);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/lock_screen"] = Post["/lock_screen"] = _ => // lock screen
            {
                showMsg("Lock Screen");
                TopeRequest request = this.Bind<TopeRequest>();
                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommandExecutor.lockScreen, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/monitor_on"] = _ => // monitor on
            {
                Console.WriteLine("OsCommandExecutor.turnMonitorOn(true);");
                bool retValue = OsCommandExecutor.turnMonitorOn(true);
                TopeResponse topeRes = new TopeResponse(retValue);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/monitor_off"] = _ => // monitor off
            {
                Console.WriteLine("OsCommandExecutor.turnMonitorOn(false);");
                bool retValue = OsCommandExecutor.turnMonitorOn(false);
                TopeResponse topeRes = new TopeResponse(retValue);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            /* ************ INPUT ************ */

            Get["/input_lock"] = _ => // lock screen
            {
                showMsg("Lock Input");
                bool retValue = OsCommandExecutor.lockInput(true);
                TopeResponse topeRes = new TopeResponse(retValue);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/input_unlock"] = _ => // lock screen
            {
                showMsg("Unlock Input");
                bool retValue = OsCommandExecutor.lockInput(false);
                TopeResponse topeRes = new TopeResponse(retValue);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            /* ****************************** */
            /* ************ TEST ************ */
            /* ****************************** */
            
            Get["/test"] = Post["/test"] = _ => // lock screen
            {
                TopeRequest request = this.Bind<TopeRequest>();
                String user = request.user;
                String pass = request.password;
                bool isAuth = PrivilegesUtil.isAuthentic(user, pass);
                showMsg(isAuth?"authenticated":"authentication failed");
                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(returnTrue, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/test.php"] = Post["/test.php"] = _ => // lock screen
            {
                
                TopeRequest request = this.Bind<TopeRequest>();
                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(returnTrue, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };
        }

        public bool returnTrue()
        {
            showMsg("Test");
            return true;
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
