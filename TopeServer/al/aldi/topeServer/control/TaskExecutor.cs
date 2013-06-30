using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.utils.general;
using TopeServer.al.aldi.utils.security;

namespace TopeServer.al.aldi.topeServer.control
{
    class TaskExecutor : ITaskExecutor
    {
        TopeRequest request = null;
        TopeResponse response = new TopeResponse();

        long timeToWait = 0;
        DateTime timeToExecute;
        IniFileUtil propertiesFile = new IniFileUtil(ProgramAdministration.getProgramPath() + Program.FILE_INI_GENERAL);


        /// <summary>
        /// Delecate Method implementing the task that need to be executed.
        /// </summary>
        /// <returns>true if task succeeded</returns>
        public delegate TopeResponse Executor();

        /// <summary>
        /// Executes function in thread. If a timer is set then it returns true, signaling
        /// that the command was accepted successfully. Otherwise returns the real value of 
        /// the outcome.
        /// </summary>
        /// <param name="d">Function to be executed</param>
        /// <param name="timeToWait">Time in milliseconds to wait before execution</param>
        /// <returns></returns>
        public ITopeResponse Execute(Func<TopeRequest, TopeResponse> d, TopeRequest request = null)
        {
            String only_current_user = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_SECURITY, Program.INI_VAR_SEC_ONLY_ACCTUAL_USER);
            if (only_current_user.Equals(Program.TRUE))
            {
                String currentUser = PrivilegesUtil.getCurrentUser();
                if (!currentUser.ToUpper().Equals(request.user.ToUpper()))
                {
                    response.message = TopeMsg.ERR_USER_NOT_ALLOWED;
                    return response;
                }
            }

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
                if (!String.IsNullOrEmpty(request.message))
                {
                    response.message = request.message;
                }
                else
                {
                    response.message = TopeMsg.ERR_AUTHENTICATION;
                }
                return response;
            }

            var thread = new Thread(
            () =>
            {
                /* if the execution time is set and its in the future */
                if (null != timeToExecute)
                {
                    /* overriding the timeToWait as the date and time sounds more accurate to use */

                    long t_timeToWait = (long)(timeToExecute - DateTime.Now).TotalMilliseconds;
                    timeToWait = t_timeToWait >= 0 ? t_timeToWait : timeToWait;
                }

                Thread.Sleep((int)timeToWait);
                try
                {

                    var exeSuccess = d(request);

                    response.success = exeSuccess.success;
                    if (response.success)
                    {
                        response.message = "Task executed successfully";
                    }
                    response.payload = exeSuccess.payload;
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
