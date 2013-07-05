using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.utils.general;
using TopeServer.al.aldi.utils.security;

namespace TopeServer.al.aldi.topeServer.control.modules
{
    class ModuleUtils
    {
        public static String MODULE_TIME_TO_EXECUTE = "/{timeToExecute}";
        static IniFileUtil propertiesFile = new IniFileUtil(ProgramAdministration.getProgramPath() + Program.FILE_INI_GENERAL);

        public static TopeRequest validate(NancyModule module)  
        {
            TopeRequest request = module.Bind<TopeRequest>();
            String user = request.user;
            String pass = request.password;
            String domain = request.domain;

            /* checking if the client is password protected, if not, no need for authentication */
            String passwordProtectedUserChecked = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_SECURITY, Program.INI_VAR_PWD_PROTECTED);
            if (passwordProtectedUserChecked.Equals(Program.TRUE))
            {
                /* checking if the user is the logged in user, otherwise return error */
                String only_current_user = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_SECURITY, Program.INI_VAR_SEC_ONLY_ACCTUAL_USER);
                if (only_current_user.Equals(Program.TRUE))
                {
                    String currentUser = PrivilegesUtil.getCurrentUser();
                    if (null == user || !currentUser.ToUpper().Equals(user.ToUpper()))
                    {
                        request.message = TopeMsg.ERR_USER_NOT_ALLOWED;
                        return request;
                    }
                }

                try
                {
                    request.authenticated = PrivilegesUtil.isAuthentic(user, pass, domain);
                }
                catch (PrincipalServerDownException e)
                {
                    request.authenticated = false;
                    request.message = e.Message;
                }
            }
            else
            {
                request.authenticated = true;
            }

            return request;
        }
    }
}
