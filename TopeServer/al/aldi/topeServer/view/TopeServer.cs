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

        public TopeServer()
        {
            InitializeComponent();
            components = new Container();
            notifyIcon1 = new NotifyIcon(components);

            try
            {
                notifyIcon1.Icon = new Icon("system_log_out.ico");
            }
            catch (Exception e)
            {
                MessageBox.Show("Icon not found");
            }
            
            contextMenu1 = new ContextMenu();
            notifyIcon1.ContextMenu = this.contextMenu1;
            exit = new MenuItem();

            notifyIcon1.Text = "Form1 (NotifyIcon example)";
            notifyIcon1.Visible = true;
            

            contextMenu1.MenuItems.AddRange(new MenuItem[] { exit });

            this.exit.Index = 0;
            this.exit.Text = "E&xit";
            this.exit.Click += new System.EventHandler(this.exit_Click);

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
            if (!FirewallUtils.checkPort(Program.FIREWALL_RULE_NAME, Program.FIREWALL_RULE_PORT))
            {
                FirewallUtils.openPort(Program.FIREWALL_RULE_NAME, Program.FIREWALL_RULE_DESC, Program.FIREWALL_RULE_PORT);
            }

            this.WindowState = FormWindowState.Minimized;


            var url = "http://localhost:" + Program.FIREWALL_RULE_PORT + "/";
            var nancy = new NancyHost(new Uri(url));
            nancy.Start();
        }

    }
}
