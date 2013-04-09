using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Win32;
using NetFwTypeLib;
using System.Net;

namespace TopeServer.al.aldi.utils.network
{
    class NetworkUtils
    {
        public static INetFwMgr WinFirewallManager()
        {
            Type type = Type.GetTypeFromCLSID(
                new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
            return Activator.CreateInstance(type) as INetFwMgr;
        }

        /// <summary>
        /// Authorizes a program to use the network without the firewall restriction.
        /// </summary>
        /// <param name="title">Title of the Rule</param>
        /// <param name="path">Path of the program to be added to the rule</param>
        /// <param name="scope">Type of connection</param>
        /// <param name="ipver">IP Version</param>
        /// <returns>bool true if succeded</returns>

        public static bool AuthorizeProgram(string title, string path, NET_FW_SCOPE_ scope, NET_FW_IP_VERSION_ ipver)
        {
            Type type = Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication");
            INetFwAuthorizedApplication authapp = Activator.CreateInstance(type)
                as INetFwAuthorizedApplication;
            authapp.Name = title;
            authapp.ProcessImageFileName = path;
            authapp.Scope = scope;
            authapp.IpVersion = ipver;
            authapp.Enabled = true;

            INetFwMgr mgr = WinFirewallManager();
            try
            {
                mgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(authapp);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a new rule and returns it.
        /// </summary>
        /// <returns>New instance of a rule</returns>
        public static INetFwRule getNewFirewallRule()
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            return firewallRule;
        }

        public static bool openPort(String name, String description, int port)
        {
            INetFwRule firewallRule = getNewFirewallRule();
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRule.Description = "Used to allow TopeServer to get connection from outsite of your computer.";
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            firewallRule.Enabled = true;
            firewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            firewallRule.LocalPorts = port.ToString();
            firewallRule.InterfaceTypes = "All";
            firewallRule.Name = name;

            INetFwPolicy2 firewallPolicy = getFirewallPolicy();
            firewallPolicy.Rules.Add(firewallRule);

            return true;
        }


        /// <summary>
        /// Checks whether the port and the rule exists.
        /// </summary>
        /// <param name="ruleName">Rule name</param>
        /// <param name="port">Port number</param>
        /// <returns></returns>
        public static bool checkPort(String ruleName, int port)
        {
            INetFwPolicy2 firewallPolicy = getFirewallPolicy();
            if (firewallPolicy.Rules.Count > 0)
            {
                try
                {
                    INetFwRule firewallRule = firewallPolicy.Rules.Item(ruleName);
                    return firewallRule.LocalPorts.Contains(port.ToString());
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the policy manager
        /// </summary>
        /// <returns>Policy</returns>
        private static INetFwPolicy2 getFirewallPolicy()
        {
            return (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
        }


        /// <summary>
        /// Retrieves the local ip address
        /// </summary>
        /// <returns>IP Address</returns>
        public static String getIpAddress()
        {
            var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            var ip = (
                       from addr in hostEntry.AddressList
                       where addr.AddressFamily.ToString() == "InterNetwork"
                       select addr.ToString()
                ).FirstOrDefault();
            return ip;
        }

    }
}