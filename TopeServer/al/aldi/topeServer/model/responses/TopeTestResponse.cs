using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.model.responses
{
    public class TopeTestResponse : TopeResponse
    {
        public TopeTestResponse()
        {

        }

        public class TestClassWithStringMessage
        {
            public String testMessage {get; set;}
        }
    }
}
