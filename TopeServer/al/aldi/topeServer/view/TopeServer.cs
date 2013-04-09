using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopeServer.al.aldi.utils.network;
using Nancy;
using Nancy.Hosting.Self;
using TopeServer.al.aldi.utils.network;

namespace TopeServer
{
    public partial class TopeServer : Form
    {
        
        NotifyIcon notifyIcon1;
        ContextMenu contextMenu1;
        MenuItem exit;
        MenuItem ipAddress;

        public TopeServer()
        {
            InitializeComponent();
            components = new Container();
            notifyIcon1 = new NotifyIcon(components);

            try
            {
                notifyIcon1.Icon = Properties.Resources.system_log_out;
            }
            catch (Exception e)
            {
                MessageBox.Show("Icon not found");
            }
            
            contextMenu1 = new ContextMenu();
            notifyIcon1.ContextMenu = this.contextMenu1;
            exit = new MenuItem();
            ipAddress = new MenuItem();

            notifyIcon1.Text = "TopeServer (Running)";
            notifyIcon1.Visible = true;
            

            contextMenu1.MenuItems.AddRange(new MenuItem[] { ipAddress, exit });

            exit.Index = 0;
            exit.Text = "E&xit"; // ------- EXIT MENU
            exit.Click += new System.EventHandler(this.exit_Click);

            
            ipAddress.Text = "IP: " + NetworkUtils.getIpAddress(); // ------- IP ADDRESS MENU
            ipAddress.Enabled = false;

            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Double Click");
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
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

            MessageBox.Show("Test");
        }

        private void TopeServer_Load(object sender, EventArgs e)
        {
            if (!NetworkUtils.checkPort(Program.FIREWALL_RULE_NAME, Program.FIREWALL_RULE_PORT))
            {
                NetworkUtils.openPort(Program.FIREWALL_RULE_NAME, Program.FIREWALL_RULE_DESC, Program.FIREWALL_RULE_PORT);
            }

            this.WindowState = FormWindowState.Minimized;


            var url = "http://localhost:" + Program.FIREWALL_RULE_PORT + "/";
            var nancy = new NancyHost(new Uri(url));
            try
            {
                nancy.Start();
            }
            catch (System.Net.HttpListenerException excp1)
            {
                MessageBox.Show("Server cound not be started. Port in use!");
            }
        }

    }
}
