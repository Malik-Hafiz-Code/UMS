using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UMS.Users
{
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUser _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID=UserID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _User=clsUser.FindByUserID(_UserID);
            if (_User==null)
            {
                MessageBox.Show("Not found User", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            _User.UserID = _UserID;
            lblUserID.Text = _UserID.ToString();
            lblUsername.Text = _User.Username.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtOldPassword.Text==""||txtNewPassword.Text==""
                ||txtConfirmPassword.Text=="")
            {
                MessageBox.Show("Fill records","Error"
                    ,MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtOldPassword.Text!=_User.Password)
            {
                MessageBox.Show("Old Password is false", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtConfirmPassword.Text!=txtNewPassword.Text)
            {
                MessageBox.Show("Confirm Password is not equal new Password", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (clsUser.ChangePassword(_User.UserID,txtNewPassword.Text))
            {
                MessageBox.Show("Update Password", "Succeeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtConfirmPassword.Clear();
                txtNewPassword.Clear();
                txtOldPassword.Clear();
            }
            else
            {
                MessageBox.Show("Password was not updated.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
