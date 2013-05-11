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
            Get["/"] = Post["/"] = _ => "TopeServer running..."; // default route

            /* ************ POWER ************ */

            Get["/hibernate"] = Post["/hibernate"] = _ => // hibernating pc
            {
                showMsg("Hibernate");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.hibernatePC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/standby"] = Post["/standby"] = _ => // standby
            {
                showMsg("Stand By");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.standbyPC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/poweroff"] = Post["/poweroff"] = _ => // suspend pc
            {
                showMsg("Power Off");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.powerOffPC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/restart"] = Post["/restart"] = _ => // restart pc
            {
                showMsg("Restart");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.restartPC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/logoff"] = Post["/logoff"] = _ => // logoff pc
            {
                showMsg("Log off");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.logOffPC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/lock_screen"] = Post["/lock_screen"] = _ => // lock screen
            {
                showMsg("Lock Screen");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.lockScreen, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/monitor_on"] = Post["/monitor_on"] = _ => // monitor on
            {
                showMsg("Monitor On");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.turnMonitorOn, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/monitor_off"] = Post["/monitor_off"] = _ => // monitor off
            {
                showMsg("Monitor Off");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.turnMonitorOff, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            /* ************ INPUT ************ */

            Get["/input_lock"] = Post["/input_lock"] = _ => // lock screen
            {
                showMsg("Lock Input");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.lockInput, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/input_unlock"] = Post["/input_unlock"] = _ => // lock screen
            {
                showMsg("Unlock Input");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(OsCommands.unlockInput, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            /* ****************************** */
            /* ************ TEST ************ */
            /* ****************************** */
           
            Get["/test"] = Post["/test"] = _ => // lock screen
            {
                TopeRequest request = ModuleUtils.validate(this);
                
                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(returnTrue, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/test.aspx"] = Post["/test.aspx"] = _ => // apsx
            {
                
                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("<h1>OK</h1>");

            };

            Get["/test.php"] = Post["/test.php"] = _ => // lock screen
            {

                TopeRequest request = new TopeRequest();//this.Bind<TopeRequest>();
                request.success = true;
                TaskExecutor te = new TaskExecutor();
                TopeResponse topeRes = te.Execute(returnTrue, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };
        }

        public bool returnTrue(TopeRequest request)
        {
            showMsg("TestMsg: "+request.message);
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
