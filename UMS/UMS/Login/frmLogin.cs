using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UMS.Global;

namespace UMS
{
    
    public partial class frmLogin : Form
    {
        
        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {     
            if (txtUsername.Text.Trim().Length<=0||txtPassword.Text.Trim().Length<=0)
            {
                MessageBox.Show("Please enter username and password.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            clsUser user = clsUser.FindByUsernameAndPassword(txtUsername.Text.Trim(), txtPassword.Text.Trim());
            if (user != null)
            {
                if (!user.IsActive)
                {
                    MessageBox.Show("This account is inactive.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                clsGlobal.CurrentUser=user;
                this.Hide();
                frmMain frm = new frmMain(this);
                frm.ShowDialog();
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
            }
            else
                MessageBox.Show("Invalid Username or Password", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
                txtPassword.PasswordChar='\0';
          //  txtPassword.UseSystemPasswordChar=false; //2
            else
                txtPassword.PasswordChar='*';
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
           
        }
    }
}
