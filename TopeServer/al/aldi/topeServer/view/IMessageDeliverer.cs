using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer.view
{
    public interface IMessageDeliverer
    {
        void showMsg(String title, String msg);
        void showMsg(String msg);
    }
}

