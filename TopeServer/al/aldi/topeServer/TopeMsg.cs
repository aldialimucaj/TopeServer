using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopeServer.al.aldi.topeServer
{
    class TopeMsg
    {
        public static string ERR_AUTHENTICATION = "User not authenticated or empty credentials";
        public static string ERR_USER_NOT_ALLOWED = "User is not allowed to send actions. Only the current logged in user is.";
    }
}
