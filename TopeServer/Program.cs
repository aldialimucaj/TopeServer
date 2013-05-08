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
using System.Security.Cryptography.X509Certificates;
using TopeServer.al.aldi.utils.general;

namespace TopeServer
{
    class Program
    {

        /* Port switching for debugging purposes */
#if DEBUG
        public const int FIREWALL_RULE_PORT = 8181;
#else
        public const int FIREWALL_RULE_PORT = 8080;
#endif

        public const String FIREWALL_RULE_NAME = "TopeClient Firewall Rule";
        public const String FIREWALL_RULE_DESC = "TopeClient Firewall Rule";
        public const String FILE_INI_GENERAL = "/TopeServer.ini";

        public const String INI_VAR_URL_BOUND = "url_bound";
        public const String INI_VAR_CERT_HASH = "ssl_cert_hash";

        public const String TRUE  = "true";
        public const String FALSE = "false";


        IniFileUtil propertiesFile = new IniFileUtil(ProgramAdministration.getProgramPath() + FILE_INI_GENERAL);
        
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

        public static String initSecurity()
        {
            X509Certificate2 certificate = EncryptionUtils.GenerateCertificate("TopeServerCert");
            EncryptionUtils.InstallCertificate(certificate, StoreName.Root);
            EncryptionUtils.InstallCertificate(certificate, StoreName.TrustedPublisher);
            EncryptionUtils.InstallCertificate(certificate, StoreName.My);
            NetworkUtils.UnBindCertificateCmd(FIREWALL_RULE_PORT); // unbind existing certificates on listening port
            return NetworkUtils.BindCertificateCmd(certificate, FIREWALL_RULE_PORT);
        }

        /// <summary>
        /// Reading the necessary parameters at startup and reacting upon the values.
        /// </summary>
        private void readParameters()
        {
            String url_bound = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_URL_BOUND);
            if (!url_bound.Equals(TRUE))
            {
                initSecurity();
                bool b = propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_URL_BOUND, TRUE);
            }
            
        }

        [STAThread]
        static void Main(string[] args)
        {

            if (WIDNOWS_FORM)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Program p = new Program();
                p.readParameters();
                p.startServer();
            }
            else
            {
                /* WINDOWS SERVICE */
            }

        }
    }
}
