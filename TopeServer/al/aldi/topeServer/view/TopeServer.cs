using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopeServer.al.aldi.utils.security;
using Nancy;
using Nancy.Hosting.Self;
using TopeServer.al.aldi.topeServer.view;
using Nancy.TinyIoc;
using TopeServer.al.aldi.utils.general;
using System.Diagnostics;

namespace TopeServer
{
    public partial class TopeServer : Form, IMessageDeliverer
    {
        
        NotifyIcon toolbarNotifyIcon;
        ContextMenu mainToolbarIconMenu;
        MenuItem exit;

        MenuItem updates;

        MenuItem ipAddress;

        MenuItem generalMenu;
        MenuItem showPopupMsg;
        MenuItem openUploadFolder;

        MenuItem securityMenu;
        MenuItem initSecurity;
        MenuItem justThisUser;
        MenuItem passwordProtectedUser;
        IniFileUtil propertiesFile = new IniFileUtil(ProgramAdministration.getProgramPath() + Program.FILE_INI_GENERAL);
        private int hostPort = 0;


        public TopeServer()
        {
            InitViews();
        
            var container = TinyIoCContainer.Current;
            container.Register<IMessageDeliverer>(this);
        }

        private void initVariables()
        {
            String hostPortSetting = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_GENERAL, Program.INI_VAR_HOST_PORT);
            if (!hostPortSetting.Equals(""))
            {
                hostPort = Convert.ToInt32(hostPortSetting);
            }
        }

        private void InitViews()
        {
            InitializeComponent();
            components = new Container();
            toolbarNotifyIcon = new NotifyIcon(components);

            toolbarNotifyIcon.Icon = Properties.Resources.system_log_out;

            mainToolbarIconMenu = new ContextMenu();
            toolbarNotifyIcon.ContextMenu = this.mainToolbarIconMenu;
            exit = new MenuItem();
            updates = new MenuItem();
            ipAddress = new MenuItem();

            generalMenu = new MenuItem();
            generalMenu.Text = "General";
            showPopupMsg = new MenuItem();
            openUploadFolder = new MenuItem();

            showPopupMsg.Text = "Show Popup Messages";
            showPopupMsg.Click += new System.EventHandler(this.showPopupMsg_Click);
            String showPopupMsgChecked = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_GENERAL, Program.INI_VAR_G_SHOW_POPUP);
            showPopupMsg.Checked = showPopupMsgChecked.Equals(Program.TRUE);

            openUploadFolder.Text = "Open Upload Folder";
            openUploadFolder.Click += new System.EventHandler((s, e) => { Process.Start(IOUtils.GetUserHomeFolder()); });

            generalMenu.MenuItems.AddRange(new MenuItem[] { showPopupMsg, openUploadFolder });
            
            securityMenu = new MenuItem();
            initSecurity = new MenuItem();
            justThisUser = new MenuItem();
            passwordProtectedUser = new MenuItem();

            mainToolbarIconMenu.MenuItems.AddRange(new MenuItem[] { updates, securityMenu, generalMenu, ipAddress, exit });
            
            //exit.Index = 0;
            exit.Text = "Exit";
            exit.Click += new System.EventHandler(this.exit_Click);

            updates.Text = "Check Updates";
            updates.Click += new System.EventHandler(this.updates_Click);

            ipAddress.Text = NetworkUtils.getIpAddress() + ":" + hostPort;
            ipAddress.Enabled = false;


            securityMenu.Text = "Security";
            securityMenu.MenuItems.AddRange(new MenuItem[] { initSecurity, justThisUser, passwordProtectedUser });

            initSecurity.Text = "Renew Encryption";
            initSecurity.Click += new System.EventHandler(this.initSecurity_Click);

            justThisUser.Text = "Only current user";
            justThisUser.Click += new System.EventHandler(this.justThisUser_Click);
            String justThisUserChecked = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_SECURITY, Program.INI_VAR_SEC_ONLY_ACCTUAL_USER);
            justThisUser.Checked = justThisUserChecked.Equals(Program.TRUE);

            passwordProtectedUser.Text = "Password Protected";
            passwordProtectedUser.Click += new System.EventHandler(this.passwordProtectedUser_Click);
            String passwordProtectedUserChecked = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_SECURITY, Program.INI_VAR_PWD_PROTECTED);
            passwordProtectedUser.Checked = passwordProtectedUserChecked.Equals(Program.TRUE);

            toolbarNotifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            
        }

        private void updates_Click(object sender, EventArgs e)
        {
            String updateUrl = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_GENERAL, Program.INI_VAR_UPDATE_URL);
            ProgCommandExecutor.openBrowserWithUrl(updateUrl);
        }

        public void showIcon(String msg = "")
        {
            if (null != toolbarNotifyIcon)
            {
                toolbarNotifyIcon.Text = "TopeServer (Running)";
                toolbarNotifyIcon.Visible = true;
            }
        }

        public void hideIcon()
        {
            if (null != toolbarNotifyIcon)
            {
                toolbarNotifyIcon.Text = "TopeServer (Stopped)";
                toolbarNotifyIcon.Visible = false;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Congatulations, you just performed a successful double click!");
        }

        

        private void exit_Click(object sender, EventArgs e)
        {
            this.toolbarNotifyIcon.Visible = false;
            this.Close();
            Environment.Exit(0);
        }

        private void initSecurity_Click(object sender, EventArgs e)
        {
            String output = Program.initSecurity(hostPort);
            showMsg(output);
        }

        private void justThisUser_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem) sender;
            item.Checked = !item.Checked;
            
            propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_SECURITY, Program.INI_VAR_SEC_ONLY_ACCTUAL_USER, item.Checked?Program.TRUE:Program.FALSE);
        }

        private void showPopupMsg_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            item.Checked = !item.Checked;

            propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_GENERAL, Program.INI_VAR_G_SHOW_POPUP, item.Checked ? Program.TRUE : Program.FALSE);
        }

        private void passwordProtectedUser_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            item.Checked = !item.Checked;

            propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_SECURITY, Program.INI_VAR_PWD_PROTECTED, item.Checked ? Program.TRUE : Program.FALSE);
        }

        private void TopeServer_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.toolbarNotifyIcon.ContextMenu = mainToolbarIconMenu;
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                this.toolbarNotifyIcon.Visible = true;
                this.Hide();

            }
        }

        private void TopeServer_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();

            MessageBox.Show("Dobule Click - Still to be implemented");
        }

        // <Start>
        // Starting point for View when it get loaded
        private void TopeServer_Load(object sender, EventArgs e)
        {
            // initialize the variables form TopeServer.ini file
            initVariables();
            ipAddress.Text = NetworkUtils.getIpAddress() + ":" + hostPort;

            if (!NetworkUtils.checkPort(Program.FIREWALL_RULE_NAME, hostPort))
            {
                NetworkUtils.openPort(Program.FIREWALL_RULE_NAME, Program.FIREWALL_RULE_DESC, hostPort);
            }

            this.WindowState = FormWindowState.Minimized;


            var url = "https://localhost:" + hostPort + "/";
            var nancy = new NancyHost(new Uri(url));
            try
            {
                nancy.Start();
                showPopup("Tope Server started", NetworkUtils.getIpAddress() + ":" + hostPort);
                /* init the messangers */
            }
            catch (System.Net.HttpListenerException excp1)
            {
                MessageBox.Show("Server cound not be started. Port in use!\n"+excp1.Message);
            }
        }

        public void showPopup(String title, String msg)
        {
            String defaultVar = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_GENERAL, Program.INI_VAR_G_SHOW_POPUP);
            if (defaultVar.Equals(Program.TRUE))
            {
                this.toolbarNotifyIcon.BalloonTipText = msg;
                this.toolbarNotifyIcon.BalloonTipTitle = title;
                this.toolbarNotifyIcon.Icon = Properties.Resources.system_log_out;
                this.toolbarNotifyIcon.Visible = true;
                this.toolbarNotifyIcon.ShowBalloonTip(2);
            }
        }


        public void showMsg(string title, string msg)
        {
            if (msg.Equals("ping"))
            {
                return;
            }
            showPopup(title, msg);
        }

        public void showMsg(string msg)
        {
            showMsg("TopeServer", msg);
        }
    }
}
