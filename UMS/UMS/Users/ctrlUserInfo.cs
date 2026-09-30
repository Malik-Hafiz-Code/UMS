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
    public partial class ctrlUserInfo : UserControl
    {
        private int _UserID;
        private clsUser _User;
        public ctrlUserInfo()
        {
            InitializeComponent();
        }
        public void Reset()
        {
            ctrlPersonInfo1.Reset();
            lblUserID.Text="???";
            lblUsername.Text="???";
            lblRole.Text="???";
            lblIsActive.Text="???";
            _UserID=-1;
            _User=null;
        }
        public int UserID { get { return _UserID; } }
        public clsUser User {  get { return _User; } }

        private void _FillUserInfo()
        {
            ctrlPersonInfo1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text=_UserID.ToString();
            lblUsername.Text=_User.Username;
            lblRole.Text=_User.Role.ToString();
            lblIsActive.Text=_User.IsActive?"True":"False";
        }
        public void LoadUserInfo(int UserID)
        {
            Reset();
            _User=clsUser.FindByUserID(UserID);
            if (_User == null)
            {
                MessageBox.Show(
                    "User ID [" + UserID + "] was not found.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }
            _UserID = UserID;
            _FillUserInfo();
        }
    }
}
