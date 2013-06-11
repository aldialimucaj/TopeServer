using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using TopeServer.al.aldi.topeServer.control.db.contexts;
using TopeServer.al.aldi.topeServer.control.db.tables;
using TopeServer.al.aldi.topeServer.model;

namespace TopeServer.al.aldi.topeServer.control
{
    class TaskManager
    {
        private static TaskManager taskManager = null;
        private TopeActionContext tac = new TopeActionContext();
        private TopeRequestDAO trDAO = null;

        public static TaskManager getInstance()
        {
            if (null == taskManager)
            {
                taskManager = new TaskManager();
            }
            return taskManager;
        }

        private TaskManager()
        {
            trDAO = new TopeRequestDAO(tac);
        }

        public bool addRequest(TopeRequest request)
        {
            TopeAction action = TopeActionDAO.getAction(request.method);
            if (null == action)
            {
                //TODO: Take care of not existing action
                return false;
            }
            else if (String.IsNullOrEmpty(action.active) && action.active.Equals(false.ToString()))
            {
                //TODO: Take care of incative actions
                return false;
            }
            else
            {
                tac.requests.Add(request);
                return 0 < tac.SaveChanges();
            }

        }

        public bool updateRequest(TopeRequest request)
        {
            return 0 < tac.SaveChanges();
        }

        public void startExecutor()
        {
            Thread thread = new Thread(requestPoolChecker);
            thread.Start();

        }

        private void requestPoolChecker()
        {
            while (true)
            {
                List<TopeRequest> activeRequests = trDAO.getRepeatableRequests();
                foreach (TopeRequest request in activeRequests)
                {
                    TaskExecutor te = new TaskExecutor();
                    TopeAction ta = TopeActionDAO.getAction(request);
                    Type t = Type.GetType(ta.module);
                    MethodInfo method = t.GetMethod(ta.method, BindingFlags.Static | BindingFlags.Public);
                    var input = Expression.Parameter(typeof(TopeRequest), "input");
                    Func<TopeRequest, bool> result = Expression.Lambda<Func<TopeRequest, bool>>(Expression.Call(method, input), input).Compile();
                    ITopeResponse topeRes = te.Execute(result, request);
                    request.executed++;
                    tac.SaveChanges();
                }
                Thread.Sleep(5000);
            }
        }
    }
}
