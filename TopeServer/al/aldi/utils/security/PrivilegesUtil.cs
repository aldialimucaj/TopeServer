using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using TopeServer.al.aldi.utils.general;

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
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Machine))
                {
                    // validate the credentials
                    isAuthentic = pc.ValidateCredentials(user, password);
                }
            }
            catch (Exception e)
            {
                TopeLogger.Log(e.StackTrace);
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
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
                {
                    // validate the credentials
                    isAuthentic = pc.ValidateCredentials(user, password);
                }
            }
            catch (Exception e)
            {
                TopeLogger.Log(e.StackTrace);
            }
            return isAuthentic;
        }

        public static String getCurrentUser()
        {
            string userName = Environment.UserName;
            return userName;
        }

    }
}
