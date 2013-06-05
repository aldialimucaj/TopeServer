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
using TopeServer.al.aldi.topeServer.control.db.tables;
using System.Reflection;
using System.Linq.Expressions;
using TopeServer.al.aldi.topeServer.model.responses;

namespace TopeServer
{
    public class OsControlModule : NancyModule
    {
        public const String MODULE_NAME = "OsControlModule";
        public IMessageDeliverer deliverer;
               
        public OsControlModule()
        {
            autoInitCommands();
            extraInitCommands();

            initControllers();
            var container = TinyIoCContainer.Current;
            IMessageDeliverer mdl = container.Resolve<IMessageDeliverer>();
            setDeliverer(mdl);
        }

        

        private void autoInitCommands()
        {
            TaskManager taskManager = TaskManager.getInstance();
            List<TopeAction> actions = getFilteredActions(TopeActionDAO.getAllActions());
            foreach (TopeAction ta in actions)
            {
                Get[ta.commandFullPath] = Post[ta.commandFullPath] = _ => // generic
                {
                    

                    showMsg(ta.method);
                    TopeRequest request = ModuleUtils.validate(this);
                    taskManager.addRequest(request);

                    /* starting execution */
                    ITaskExecutor taskExecutor = new TaskExecutor();
                    Type t = Type.GetType(ta.module);
                    MethodInfo method = t.GetMethod(ta.method, BindingFlags.Static | BindingFlags.Public);
                    var input = Expression.Parameter(typeof(TopeRequest), "input");
                    Func<TopeRequest, bool> result = Expression.Lambda<Func<TopeRequest, bool>>(Expression.Call( method, input), input).Compile();
                    var topeRes = taskExecutor.Execute(result, request);
                    /* execution finished */

                    /* updating the request */
                    request.success = topeRes.isSuccessful();
                    request.executed++;
                    taskManager.updateRequest(request);

                    TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                    return nego.Response;
                };
            }
        }

        private void extraInitCommands()
        {
            /* ************ INPUT ************ */
            
            Get["/os/lockInput"] = Post["/os/lockInput"] = _ => // lock screen
            {
                showMsg("Lock Input");
                TopeRequest request = ModuleUtils.validate(this);



                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.lockInput, request);

                if (request.authenticated)
                {
                    topeRes.setSuccess(OsCommandExecutor.lockInput());//TODO: Remove this. It is just a workaround the bug
                }

                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/os/unlockInput"] = Post["/os/unlockInput"] = _ => // lock screen
            {
                showMsg("Unlock Input");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.unlockInput, request);

                if (request.authenticated)
                {
                    topeRes.setSuccess(OsCommandExecutor.unlockInput());//TODO: Remove this. It is just a workaround the bug
                }

                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            /* ****************************** */
            /* ************ TEST ************ */
            /* ****************************** */

            Get["/os/test"] = Post["/os/test"] = _ => // lock screen
            {
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(returnTrue, request);

                TopeTestResponse.TestClassWithStringMessage payload = new TopeTestResponse.TestClassWithStringMessage();
                payload.testMessage = "master of generics";
                topeRes.setPayload(payload);
                
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/os/test2"] = Post["/os/test2"] = _ => // lock screen
            {
                TopeRequest request = ModuleUtils.validate(this);
                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(returnTrue, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/os/test.aspx"] = Post["/os/test.aspx"] = _ => // apsx
            {

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithModel("<h1>OK</h1>");

            };

            Get["/os/test.php"] = Post["/os/test.php"] = _ => // lock screen
            {

                TopeRequest request = new TopeRequest();//this.Bind<TopeRequest>();
                request.success = true;
                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(returnTrue, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

        }

        private List<TopeAction> getFilteredActions(List<TopeAction> actions)
        {
            List<TopeAction> filteredActions = new List<TopeAction>();
            foreach (TopeAction ta in actions)
            {
                /* skipping extra methods */
                switch (ta.method)
                {
                    case "lockInput":
                    case "unlockInput":
                        continue;

                }
                filteredActions.Add(ta);
            }
            return filteredActions;
        }

        private void initControllers()
        {
            
        }







        /* ********************************** */
        /* ************ OBSOLETE ************ */
        /* ********************************** */

        private void initCommands()
        {
            Get["/"] = Post["/"] = _ => "TopeServer running..."; // default route

            /* ************ POWER ************ */

            Get["/hibernate"] = Post["/hibernate"] = _ => // hibernating pc
            {
                showMsg("Hibernate");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.hibernatePC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/standby"] = Post["/standby"] = _ => // standby
            {
                showMsg("Stand By");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.standbyPC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/poweroff"] = Post["/poweroff"] = _ => // suspend pc
            {
                showMsg("Power Off");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.powerOffPC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/restart"] = Post["/restart"] = _ => // restart pc
            {
                showMsg("Restart");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.restartPC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/logoff"] = Post["/logoff"] = _ => // logoff pc
            {
                showMsg("Log off");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.logOffPC, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;
            };

            Get["/lock_screen"] = Post["/lock_screen"] = _ => // lock screen
            {
                showMsg("Lock Screen");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.lockScreen, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/monitor_on"] = Post["/monitor_on"] = _ => // monitor on
            {
                showMsg("Monitor On");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.turnMonitorOn, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/monitor_off"] = Post["/monitor_off"] = _ => // monitor off
            {
                showMsg("Monitor Off");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.turnMonitorOff, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            /* ************ INPUT ************ */

            Get["/input_lock"] = Post["/input_lock"] = _ => // lock screen
            {
                showMsg("Lock Input");
                TopeRequest request = ModuleUtils.validate(this);

               

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.lockInput, request);
                
                if (request.authenticated)
                {
                    topeRes.setSuccess(OsCommandExecutor.lockInput());//TODO: Remove this. It is just a workaround the bug
                }

                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/input_unlock"] = Post["/input_unlock"] = _ => // lock screen
            {
                showMsg("Unlock Input");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.unlockInput, request);
                
                if (request.authenticated)
                {
                    topeRes.setSuccess( OsCommandExecutor.unlockInput());//TODO: Remove this. It is just a workaround the bug
                }
                
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            /* ************ INPUT ************ */
            Get["/sound_off"] = Post["/sound_off"] = _ => // monitor off
            {
                showMsg("Mute Sound");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.soundMute, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/sound_on"] = Post["/sound_on"] = _ => // monitor off
            {
                showMsg("Sound On");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(OsCommands.soundOn, request);
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
                ITopeResponse topeRes = te.Execute(returnTrue, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            Get["/test2"] = Post["/test2"] = _ => // lock screen
            {
                TopeRequest request = ModuleUtils.validate(this);
                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(returnTrue, request);
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
                ITopeResponse topeRes = te.Execute(returnTrue, request);
                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };
        }

        public bool returnTrue(TopeRequest request)
        {
            showMsg("TestMsg: "+request.message);
            return true;
        }

        public bool returnTrue()
        {
            showMsg("TestMsg: ");
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
