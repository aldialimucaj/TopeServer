using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TopeServer.al.aldi.topeServer.control
{
    class TaskExecutor
    {
        public delegate bool Executor();

        /// <summary>
        /// Executes function in thread
        /// </summary>
        /// <param name="d">Function to be executed</param>
        /// <param name="timeToWait">Time in milliseconds to wait before execution</param>
        /// <returns></returns>
        public bool Execute(Func<bool> d, int timeToWait = 0)
        {
            Thread.Sleep(timeToWait);
            return d();
        }
    }
}
