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

        public static bool isAuthentic(String user, String password)
        {
            bool isAuthentic = false;
            if (user == null)
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

        public static bool isAuthenticInDomain(String user, String password, String domain)
        {
            bool isAuthentic = false;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                // validate the credentials
                isAuthentic = pc.ValidateCredentials(user, password);
            }
            return isAuthentic;
        }
	
    }
}
