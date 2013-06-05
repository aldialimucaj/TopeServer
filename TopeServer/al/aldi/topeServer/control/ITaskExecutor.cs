using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.model;

namespace TopeServer.al.aldi.topeServer.control
{
    interface ITaskExecutor
    {
        ITopeResponse Execute(Func<TopeRequest, bool> d, TopeRequest request = null);
    }
}
