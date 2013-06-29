using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.model.responses
{
    public class SimpleTextResponse : TopeResponse
    {
        public SimpleTextResponse()
        {

        }

        public class ClassWithStringMessage
        {
            public String message {get; set;}
        }
    }
}
