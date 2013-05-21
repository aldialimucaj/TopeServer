using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.utils.general;

namespace TopeServer.al.aldi.topeServer.control
{
    class TaskExecutor
    {
        TopeRequest request = null;
        TopeResponse response = new TopeResponse();

        long timeToWait = 0;
        DateTime timeToExecute;
        


        /// <summary>
        /// Delecate Method implementing the task that need to be executed.
        /// </summary>
        /// <returns>true if task succeeded</returns>
        public delegate bool Executor();

        /// <summary>
        /// Executes function in thread. If a timer is set then it returns true, signaling
        /// that the command was accepted successfully. Otherwise returns the real value of 
        /// the outcome.
        /// </summary>
        /// <param name="d">Function to be executed</param>
        /// <param name="timeToWait">Time in milliseconds to wait before execution</param>
        /// <returns></returns>
        public TopeResponse Execute(Func<TopeRequest, bool> d, TopeRequest request = null)
        {
            // This was ment for acception GET requests
            if (null != request && request.authenticated)
            {
                this.request = request;
                this.timeToWait = request.timeToWait;
                this.timeToExecute = request.timeToExecute;
            }
            else
            {
                response.success = false;
                response.message = TopeMsg.ERR_AUTHENTICATION;
                return response;
            }

            var thread = new Thread(
            () =>
            {
                /* if the execution time is set and its in the future */
                if (null != timeToExecute )
                {
                    /* overriding the timeToWait as the date and time sounds more accurate to use */
                    
                    long t_timeToWait =(long) (timeToExecute - DateTime.Now).TotalMilliseconds;
                    timeToWait = t_timeToWait >= 0 ? t_timeToWait : timeToWait;
                }

                Thread.Sleep((int)timeToWait);
                try
                {
                    var exeSuccess = d(request);
                    response.success = exeSuccess;
                    if (exeSuccess)
                    {
                        response.message = "Task executed successfully";
                    }
                }
                catch (Exception e)
                {
                    response.message = "[ERROR]: " + e.GetBaseException().Message;
                }
            });

            /* Starting the thread */
            thread.Start();

            /* If the time to wait for the taks is smaller then a couple of seconds   */
            /* then the user can wait for it to finish. Otherwise just execute it and */
            /* return true to signalize that the command was accepted successfully    */
            if (isItWorthWaiting()) /* TIME TO WAIT || TIME TO EXECUTE */
            {
                thread.Join();
            }
            else
            {
                response.success = true;
            }

            return response;
        }

        private bool isItWorthWaiting()
        {
            return !(timeToWait != 0 || timeToExecute != null);
        }
    }


}
