using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using TopeServer.al.aldi.utils.security;
using NetFwTypeLib;
using System.Windows.Forms;

namespace TopeServer
{
    class Program
    {
        public const String FIREWALL_RULE_NAME = "TopeClient Firewall Rule";
        public const String FIREWALL_RULE_DESC = "TopeClient Firewall Rule";
        
        /* Port switching for debugging purposes */
#if DEBUG 
        public const int FIREWALL_RULE_PORT = 8081;
#else
        public const int FIREWALL_RULE_PORT = 8080;
#endif

        private static bool WIDNOWS_FORM = true;

        TopeServer ts = new TopeServer();

        private void startServer()
        {
            if (WIDNOWS_FORM)
            {
                Application.Run(ts);
                ts.showIcon();
            }
            else
            {
                /* WINDOWS SERVICE */
            }
#if DEBUG 
        StaticConfiguration.DisableErrorTraces = false;
#endif
        }

        [STAThread]
        static void Main(string[] args)
        {

            if (WIDNOWS_FORM)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Program p = new Program();
                p.startServer();
            }
            else
            {
                /* WINDOWS SERVICE */
            }

        }
    }
}
