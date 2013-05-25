﻿using System;
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
using TopeServer.al.aldi.topeServer.control.db.tables;
using TopeServer.al.aldi.topeServer.model;
using TopeServer.al.aldi.topeServer.control.db.contexts;
using System.Reflection;
using TopeServer.al.aldi.topeServer.control.executors;
using TopeServer.al.aldi.topeServer.control;

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
        TaskManager taskManager = TaskManager.getInstance();

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

        private void reloadDatabase()
        {
            TopeActionDAO tAction = new TopeActionDAO();
            tAction.dropTable();
            tAction.createTable();
            TopeRequestDAO tRequest = new TopeRequestDAO();
            tRequest.dropTable();
            tRequest.createTable();

            addActions();
           
        }

        private void addActions(Type t, String prefix)
        {
            TopeActionDAO tAction = new TopeActionDAO();
            tAction.dropTable();
            tAction.createTable();

            TopeActionContext tac = new TopeActionContext();

            MethodInfo[] methodInfos = t.GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (MethodInfo mi in methodInfos)
            {
                TopeAction ta = new TopeAction();
                ta.active = true.ToString();
                ta.module = t.FullName;
                ta.method = mi.Name;
                ta.commandFullPath = prefix + mi.Name;
                tac.actions.Add(ta);
            }

            tac.SaveChanges();
        }

        private void startTaskManager()
        {
            taskManager.startExecutor();
        }

        private void addActions()
        {
            Type t = typeof(OsCommands);
            addActions(t, "/os/");
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
                p.reloadDatabase();
                p.startTaskManager();
                p.startServer();
            }
            else
            {
                /* WINDOWS SERVICE */
            }

        }
    }
}

