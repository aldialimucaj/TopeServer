using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using TopeServer.al.aldi.utils.network;
using NetFwTypeLib;

namespace TopeServer
{
    class Program
    {
        public const String FIREWALL_RULE_NAME = "TopeClient Firewall Rule";
        public const String FIREWALL_RULE_DESC = "TopeClient Firewall Rule";
        public const int    FIREWALL_RULE_PORT = 8080;

        static void Main(string[] args)
        {
            String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            String programName = System.IO.Path.GetFileNameWithoutExtension(path);

            if (!FirewallUtils.checkPort(FIREWALL_RULE_NAME, FIREWALL_RULE_PORT))
            {
                Console.WriteLine("Opening PORT");
                FirewallUtils.openPort(FIREWALL_RULE_NAME, FIREWALL_RULE_DESC, FIREWALL_RULE_PORT);
            }
            else
            {
                Console.WriteLine("Rule Exists");
            }

            
            var url = "http://localhost:" + FIREWALL_RULE_PORT + "/";
            Console.WriteLine(url);
            var nancy = new NancyHost(new Uri(url));
            nancy.Start();
            Console.ReadKey();

        }
    }
}
