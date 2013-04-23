using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TopeServer.al.aldi.topeServer.model;

namespace TopeServer.al.aldi.topeServer.control
{
    class TaskExecutor
    {
        TopeRequest request = null;
        TopeResponse response = new TopeResponse();

        int timeToWait = 0;
        int timeToExecute = 0;
        


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
        public TopeResponse Execute(Func<bool> d, TopeRequest request = null)
        {
            if (null != request)
            {
                this.request = request;
                this.timeToWait = request.timeToWait;
                this.timeToExecute = request.timeToExecute;
            }

            var thread = new Thread(
            () =>
            {
                Thread.Sleep(timeToWait);
                try
                {
                    var exeSuccess = d();
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
            return !(timeToWait != 0 || timeToExecute != 0);
        }
    }


}
