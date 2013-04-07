﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;
using TopeServer.al.aldi.utils.network;
using NetFwTypeLib;
using System.Windows.Forms;

namespace TopeServer
{
    class Program
    {
        public const String FIREWALL_RULE_NAME = "TopeClient Firewall Rule";
        public const String FIREWALL_RULE_DESC = "TopeClient Firewall Rule";
        public const int FIREWALL_RULE_PORT = 8080;

        private static bool WIDNOWS_FORM = true;

        [STAThread]
        static void Main(string[] args)
        {
            //String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //String programName = System.IO.Path.GetFileNameWithoutExtension(path);

            if (WIDNOWS_FORM)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TopeServer());

              
            }

        }
    }
}
