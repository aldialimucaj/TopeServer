using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.model.responses
{
    public class SynchActionResponse : TopeResponse
    {
        public class ActionList
        {
            public List<TopeAction> actions {get; set;}

            public ActionList()
            {
                actions = new List<TopeAction>();
            }
        }
    }
}
