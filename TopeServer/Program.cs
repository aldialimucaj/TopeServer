﻿using System;
using System.Diagnostics;
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
using System.IO;

namespace TopeServer
{
    class Program
    {
        public const String LOG_TAG = "Tope Sever";
        public const String LOG_APP = "Application";

        /* Port switching for debugging purposes */
#if DEBUG
        public const int FIREWALL_RULE_PORT = 8181;
#else
        public const int FIREWALL_RULE_PORT = 8503;
#endif
        private static bool WIDNOWS_FORM                    = true;

        public const String FIREWALL_RULE_NAME              = "TopeClient Firewall Rule";
        public const String FIREWALL_RULE_DESC              = "TopeClient Firewall Rule";
        public const String FOLDER_NAME_UPLOAD              = "Tope";
        public const String FILE_INI_GENERAL                = "/TopeServer.ini";
        public const String FILE_CERT_NAME                  = "TopeCert.pfx";
        public const String FILE_CERT_PASSWORD              = "TopePassword";
        public const String TOPE_CERT_NAME                  = "TopeServerCert";
        public const String TOPE_UPDATE_URL                 = "http://aldi.al/TopeServer/";

        public const String INI_VAR_DEFAULT                 = "default";
        public const String INI_VAR_URL_BOUND               = "url_bound";
        public const String INI_VAR_HOST_PORT               = "host_port";
        public const String INI_VAR_CERT_HASH               = "ssl_cert_hash";
        public const String INI_VAR_PWD_PROTECTED           = "password_protected";
        public const String INI_VAR_DB_CREATED              = "db_created";
        public const String INI_VAR_SEC_ONLY_ACCTUAL_USER   = "only_actual_user";
        public const String INI_VAR_G_SHOW_POPUP            = "show_popup_msg";
        public const String INI_VAR_UPDATE_URL              = "update_url";

        public const String TRUE                            = "true";
        public const String FALSE                           = "false";


        private IniFileUtil propertiesFile  = new IniFileUtil(ProgramAdministration.getProgramPath() + FILE_INI_GENERAL);
        private TopeServer ts               = new TopeServer();
        private TaskManager taskManager     = TaskManager.getInstance();
        private int hostPort                = 0;

        private void startServer()
        {
            if (WIDNOWS_FORM)
            {
                try
                {
                    Application.Run(ts);
                    ts.showIcon();
                    TopeLogger.Log("Tope Server started successfully");
                }
                catch (Exception e)
                {
                    TopeLogger.Error(e.StackTrace);
                }
                
            }
            else
            {
                /* WINDOWS SERVICE */
            }
#if DEBUG
            StaticConfiguration.DisableErrorTraces = false;
#endif
        }

        /// <summary>
        /// Init the SSL certificate and register it under the port and program
        /// </summary>
        /// <returns></returns>
        public static String initSecurity(int hostPort)
        {
            EncryptionUtils.RemoveCertificate(TOPE_CERT_NAME); // Clean up the old certificates
            X509Certificate2 certificate = EncryptionUtils.GenerateCertificate(TOPE_CERT_NAME);
            EncryptionUtils.SaveCertificateToFile(certificate, FILE_CERT_PASSWORD);
            EncryptionUtils.InstallCertificate(FILE_CERT_PASSWORD, StoreName.Root);
            EncryptionUtils.InstallCertificate(FILE_CERT_PASSWORD, StoreName.TrustedPublisher);
            EncryptionUtils.InstallCertificate(FILE_CERT_PASSWORD, StoreName.My);
            FileInfo fileInfo = new FileInfo(FILE_CERT_NAME);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();// deleting the certificate after installation. WARNING: Do not remove this. 
            }
            NetworkUtils.UnBindCertificateCmd(hostPort); // unbind existing certificates on listening port
            return NetworkUtils.BindCertificateCmd(certificate, hostPort);
        }

        /// <summary>
        /// Reading the necessary parameters at startup and reacting upon the values.
        /// </summary>
        private void readParameters()
        {
            // setting up default values for the topeServer.ini
            setupDefaultIni();

            String hostPortSetting = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_HOST_PORT);
            if (!hostPortSetting.Equals(""))
            {
                hostPort = Convert.ToInt32(hostPortSetting);
            }


            String url_bound = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_URL_BOUND);
            if (!url_bound.Equals(TRUE))
            {
                initSecurity(hostPort);
                bool b = propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_URL_BOUND, TRUE);
            }

            reloadDatabase();
            String database_created = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_DATABASE, INI_VAR_DB_CREATED);
            if (!database_created.Equals(TRUE))
            {
                reloadDatabase();
            }
        }

        private void setupDefaultIni()
        {
            String defaultVar = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_DEFAULT);
            if (!defaultVar.Equals(TRUE))
            {
                propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_DEFAULT, TRUE);
                propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_HOST_PORT, Convert.ToString(FIREWALL_RULE_PORT));
                propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_DEFAULT, TRUE);
                propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_SECURITY, INI_VAR_PWD_PROTECTED, FALSE);
                propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_G_SHOW_POPUP, TRUE);
                propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_GENERAL, INI_VAR_UPDATE_URL, TOPE_UPDATE_URL);
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
            bool b = propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_DATABASE, INI_VAR_DB_CREATED, TRUE);
        }

        private void addActions(Type t, String prefix)
        {
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

            t = typeof(ProgCommands);
            addActions(t, "/prog/");

            t = typeof(UtilCommands);
            addActions(t, "/util/");
        }


        [STAThread]
        static void Main(string[] args)
        {

            if (WIDNOWS_FORM)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (!EventLog.SourceExists(LOG_TAG))
                {
                    EventLog.CreateEventSource(LOG_TAG, LOG_APP);
                }

                Program p = new Program();
                try
                {
                    p.readParameters();
                    TopeLogger.Log("Starting parameters loaded successfully");
                    p.startTaskManager();
                    TopeLogger.Log("Task Manager started successfully");
                    p.startServer();
                    TopeLogger.Log("Server stopping...");
                }
                catch (Exception e)
                {
                    TopeLogger.Error(e.StackTrace);
                }
            }
            else
            {
                /* WINDOWS SERVICE */
            }
        }
    }
}

