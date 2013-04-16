using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace TopeServer.al.aldi.utils.security
{
    class PrivilegesUtil
    {
        public static bool IsElevated
        {
            get
            {
                return new WindowsPrincipal
                    (WindowsIdentity.GetCurrent()).IsInRole
                    (WindowsBuiltInRole.Administrator);
            }
        }
	
    }
}
