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

namespace TopeServer
{
    public partial class TopeServer : Form, IMessageDeliverer
    {
        
        NotifyIcon notifyIcon1;
        ContextMenu contextMenu1;
        MenuItem exit;
        MenuItem ipAddress;

        MenuItem security;
        MenuItem initSecurity;
        MenuItem justThisUser;
        IniFileUtil propertiesFile = new IniFileUtil(ProgramAdministration.getProgramPath() + Program.FILE_INI_GENERAL);


        public TopeServer()
        {
            InitViews();

            var container = TinyIoCContainer.Current;
            container.Register<IMessageDeliverer>(this);
        }

        private void InitViews()
        {
            InitializeComponent();
            components = new Container();
            notifyIcon1 = new NotifyIcon(components);

            notifyIcon1.Icon = Properties.Resources.system_log_out;

            contextMenu1 = new ContextMenu();
            notifyIcon1.ContextMenu = this.contextMenu1;
            exit = new MenuItem();
            ipAddress = new MenuItem();
            security = new MenuItem();
            initSecurity = new MenuItem();
            justThisUser = new MenuItem();

            contextMenu1.MenuItems.AddRange(new MenuItem[] { security, ipAddress, exit });

            exit.Index = 0;
            exit.Text = "E&xit";
            exit.Click += new System.EventHandler(this.exit_Click);


            ipAddress.Text = NetworkUtils.getIpAddress() + ":" + Program.FIREWALL_RULE_PORT;
            ipAddress.Enabled = false;


            security.Text = "Security";
            security.MenuItems.AddRange(new MenuItem[] {  initSecurity, justThisUser });

            initSecurity.Text = "Renew Encryption";
            initSecurity.Click += new System.EventHandler(this.initSecurity_Click);

            justThisUser.Text = "Only current user";
            justThisUser.Click += new System.EventHandler(this.justThisUser_Click);
            String justThisUserChecked = propertiesFile.IniReadValue(IniFileUtil.INI_SECTION_SECURITY, Program.INI_VAR_SEC_ONLY_ACCTUAL_USER);
            justThisUser.Checked = justThisUserChecked.Equals(Program.TRUE);

            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            
        }

        public void showIcon(String msg = "")
        {
            if (null != notifyIcon1)
            {
                notifyIcon1.Text = "TopeServer (Running)";
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Congatulations, you just performed a successful double click!");
        }

        

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void initSecurity_Click(object sender, EventArgs e)
        {
            String output = Program.initSecurity();
            showMsg(output);
        }

        private void justThisUser_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem) sender;
            item.Checked = !item.Checked;
            
            propertiesFile.IniWriteValue(IniFileUtil.INI_SECTION_SECURITY, Program.INI_VAR_SEC_ONLY_ACCTUAL_USER, item.Checked?Program.TRUE:Program.FALSE);
        }

        private void TopeServer_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.notifyIcon1.ContextMenu = contextMenu1;
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
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

        private void TopeServer_Load(object sender, EventArgs e)
        {
            if (!NetworkUtils.checkPort(Program.FIREWALL_RULE_NAME, Program.FIREWALL_RULE_PORT))
            {
                NetworkUtils.openPort(Program.FIREWALL_RULE_NAME, Program.FIREWALL_RULE_DESC, Program.FIREWALL_RULE_PORT);
            }

            this.WindowState = FormWindowState.Minimized;


            var url = "https://localhost:" + Program.FIREWALL_RULE_PORT + "/";
            var nancy = new NancyHost(new Uri(url));
            try
            {
                nancy.Start();
                showPopup("Tope Server started", NetworkUtils.getIpAddress() + ":" + Program.FIREWALL_RULE_PORT);
                /* init the messangers */
            }
            catch (System.Net.HttpListenerException excp1)
            {
                MessageBox.Show("Server cound not be started. Port in use!\n"+excp1.Message);
            }
        }

        public void showPopup(String title, String msg)
        {
            this.notifyIcon1.BalloonTipText = msg;
            this.notifyIcon1.BalloonTipTitle = title;
            this.notifyIcon1.Icon = Properties.Resources.system_log_out;
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.ShowBalloonTip(3);
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
