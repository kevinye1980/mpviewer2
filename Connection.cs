using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MPViewer
{
    
    public partial class Connection : Form
    {
        public string Server;
        public string User;
        public string Password;
        public Connection()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Server = txtMS.Text;
            this.Password = txtPassword.Text;
            this.User = txtUser.Text;

            this.Close();
        }
    }
}
