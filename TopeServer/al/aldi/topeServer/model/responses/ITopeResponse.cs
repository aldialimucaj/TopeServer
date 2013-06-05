using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.model
{
    interface ITopeResponse
    {
        bool isSuccessful();
        void setSuccess(bool success);

        object getPayload();
        void setPayload(object payload);
    }
}
