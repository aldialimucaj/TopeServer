using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.utils.security;

namespace TopeServer.al.aldi.topeServer.control.modules
{
    class ModuleUtils
    {
        public static String MODULE_TIME_TO_EXECUTE = "/{timeToExecute}";

        public static TopeRequest validate(NancyModule module)  
        {
            TopeRequest request = module.Bind<TopeRequest>();
            String user = request.user;
            String pass = request.password;
            String domain = request.domain;

            try
            {
                request.authenticated = PrivilegesUtil.isAuthentic(user, pass, domain);
            }
            catch (PrincipalServerDownException e)
            {
                request.authenticated = false;
                request.message = e.Message;
            }

            return request;
        }
    }
}
