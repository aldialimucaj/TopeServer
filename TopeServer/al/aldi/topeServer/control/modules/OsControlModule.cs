﻿using System;
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
        public const String MODULE_NAME = "ActionControlModule";
        public IMessageDeliverer deliverer;
               
        public OsControlModule()
        {
            autoInitCommands();
            extraInitCommands();

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
            /* ************ SYNCHRONIZE ACTIONS ************ */

            Get["/os/synchActions"] = Post["/os/synchActions"] = _ => // Synch actions
            {
                showMsg("Synch Actions");
                TopeRequest request = ModuleUtils.validate(this);

                TaskExecutor te = new TaskExecutor();
                ITopeResponse topeRes = te.Execute(dummyMethod, request);

                SynchActionResponse.ActionList payload = new SynchActionResponse.ActionList();
                payload.actions = TopeActionDAO.getAllActions();
                topeRes.setPayload(payload);                

                TopeResponseNegotiator nego = new TopeResponseNegotiator(Negotiate, topeRes);
                return nego.Response;

            };

            /* ****************************** */
            /* ************  OS  ************ */
            /* ****************************** */

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
            /* ************ PROG ************ */
            /* ****************************** */


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

        public bool dummyMethod(TopeRequest request)
        {
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
