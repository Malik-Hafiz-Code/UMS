using Business;
using System;
using System.Windows.Forms;

namespace UMS.Users
{
    public partial class frmAddUpdateUser : Form
    {
        private enum enMode
        {
            Add,
            Update
        }
        private enMode _Mode = enMode.Add;
        private int _UserID = -1;
        private clsUser _User;
        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.Add;
            _User = new clsUser();
            lblTitle.Text = "Add New User";
            this.Text = lblTitle.Text;
            cbRole.SelectedIndex = 0;
            btnSave.Enabled = false;
            tpLoginInfo.Enabled = false;
        }
        
        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _Mode = enMode.Update;
            lblTitle.Text = "Update User";
            this.Text = lblTitle.Text;
            ctrlPersonInfoWithFilter1.FilterEnabled = false;
        }

        private void _Reset()
        {
            lblUserID.Text = "???";
            txtUserName.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            cbRole.SelectedIndex=0;
            chkIsActive.Checked = true;
        }

        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);
            if (_User == null)
            {
                MessageBox.Show(
                    "User Not Found",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                this.Close();
                return;
            }

            ctrlPersonInfoWithFilter1.LoadInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.Username;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            cbRole.Text =_User.Role.ToString();
            chkIsActive.Checked = _User.IsActive;
            tpLoginInfo.Enabled = true;
            btnSave.Enabled = true;
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ctrlPersonInfoWithFilter1.SelectedPersonInfo == null)
            {
                MessageBox.Show(
                    "Please select a valid Person first.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ctrlPersonInfoWithFilter1.FilterFocus();
                return;
            }
            _User.PersonID = ctrlPersonInfoWithFilter1.PersonID;
            tpLoginInfo.Enabled = true;
            tcUserInfo.SelectedTab = tpLoginInfo;
            btnSave.Enabled = true;
            ctrlPersonInfoWithFilter1.FilterEnabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool _Validation()
        {
            if (_Mode == enMode.Add)
            {
                if (clsUser.IsUserExistByPersonID(_User.PersonID))
                {
                    MessageBox.Show(
                        "This Person already has a User account.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }

            if (txtUserName.Text.Trim() == "")
            {
                MessageBox.Show("Fill username", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Fill password", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtConfirmPassword.Text.Trim() == "")
            {
                MessageBox.Show("Fill confirm password", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                MessageBox.Show("Confirm password is not equal password",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtConfirmPassword.Focus();
                return false;
            }

            if (cbRole.Text == "Student")
            {
                if (!clsStudent.IsStudentByPersonID(_User.PersonID))
                {
                    MessageBox.Show(
                        "This Person is not a Student.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }

            else if (cbRole.Text == "Doctor")
            {
                if (!clsDoctor.IsDoctorByPersonID(_User.PersonID))
                {
                    MessageBox.Show(
                        "This Person is not a Doctor.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }

            else if (cbRole.Text == "Admin")
            {
                if (clsStudent.IsStudentByPersonID(_User.PersonID) ||
                    clsDoctor.IsDoctorByPersonID(_User.PersonID))
                {
                    MessageBox.Show(
                        "This Person is already a Student or Doctor.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_Validation())
                return;
            _User.Username = txtUserName.Text.Trim();
            _User.Password = txtPassword.Text;
            _User.Role = (clsUser.enRole)Enum.Parse(
                  typeof(clsUser.enRole),cbRole.Text);
            _User.IsActive = chkIsActive.Checked;
            if (_Mode == enMode.Add)
            {
                if (_User.Save())
                {
                    _UserID = _User.UserID;
                    lblUserID.Text = _UserID.ToString();
                    MessageBox.Show(
                        $"User [{_UserID}] added successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    _Mode = enMode.Update;
                    lblTitle.Text = "Update User";
                    this.Text = lblTitle.Text;
                    ctrlPersonInfoWithFilter1.FilterEnabled = false;
                }
                else
                {
                    MessageBox.Show(
                        "User was not added.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                if (_User.Save())
                {
                    MessageBox.Show(
                        "User updated successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "User was not updated.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}