using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
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

        public static bool isAuthenticLocal(String user, String password)
        {
            bool isAuthentic = false;
            if (user == null || password == null)
            {
                return false;
            }
            using (PrincipalContext pc = new PrincipalContext(ContextType.Machine))
            {
                // validate the credentials
                isAuthentic = pc.ValidateCredentials(user, password);
            }
            return isAuthentic;
        }

        public static bool isAuthentic(String user, String password, String domain)
        {
            bool isAuthentic = false;

            if (String.IsNullOrEmpty(domain))
            {
                return isAuthenticLocal(user, password);
            }

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                // validate the credentials
                isAuthentic = pc.ValidateCredentials(user, password);
            }
            return isAuthentic;
        }

    }
}
