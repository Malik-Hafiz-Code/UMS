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
    public partial class frmListUser : Form
    {
        private DataTable _dtUser = clsUser.GetAllUsers();
        private void _RefreshData()
        {
            _dtUser=clsUser.GetAllUsers();
            dgvUsers.DataSource = _dtUser;
            lblRecords.Text=dgvUsers.RowCount.ToString();
        }
        public frmListUser()
        {
            InitializeComponent();
        }

        private void frmListUser_Load(object sender, EventArgs e)
        {
            _RefreshData();
            cbFilterBy.SelectedIndex=0;

            if(dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText="User ID";
                dgvUsers.Columns[0].Width=80;
                dgvUsers.Columns[1].HeaderText="Person ID";
                dgvUsers.Columns[1].Width=80;
                dgvUsers.Columns[2].HeaderText="Username";
                dgvUsers.Columns[2].Width=120;
                dgvUsers.Columns[3].HeaderText="Full Name";
                dgvUsers.Columns[3].Width=200;
                dgvUsers.Columns[4].HeaderText="Role";
                dgvUsers.Columns[4].Width=120;
                dgvUsers.Columns[5].HeaderText="Is Active";
                dgvUsers.Columns[5].Width=80;
            }
        }

        private void benClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "IsActive")
            {
                txtFilter.Visible= false;
                cbIsActive.Visible = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            }

            else
            {
                txtFilter.Visible = (cbFilterBy.Text !="None");
                cbIsActive.Visible = false;

                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "UserID":
                    FilterColumn="UserID";
                    break;
                case "PersonID":
                    FilterColumn="PersonID";
                    break;
                case "Username":
                    FilterColumn="Username";
                    break;
                case "FullName":
                    FilterColumn="Full Name";
                    break;
                case "Role":
                    FilterColumn="Role";
                        break;
                default:
                    FilterColumn="None";
                    break;
            }
            if (txtFilter.Text.Trim()==""||FilterColumn=="None")
            {
                _dtUser.DefaultView.RowFilter="";
                lblRecords.Text=dgvUsers.RowCount.ToString();
                return;
            }
            if (FilterColumn=="UserID"||FilterColumn=="PersonID")
                _dtUser.DefaultView.RowFilter=string.Format(
                    "[{0}]={1}", FilterColumn, txtFilter.Text.Trim());
            else
                _dtUser.DefaultView.RowFilter=string.Format(
                    "[{0}] like '{1}%'", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text=dgvUsers.RowCount.ToString();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterValue = cbIsActive.Text;
            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue="1";
                    break;
                default:
                    FilterValue="0";
                    break;
            }
            if (FilterValue=="All")
                _dtUser.DefaultView.RowFilter="";
            else
                _dtUser.DefaultView.RowFilter=string.Format(
                    "[{0}]={1}", "IsActive", FilterValue);
            lblRecords.Text=dgvUsers.RowCount.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
               "Are you sure you want to become user inactive ["
               + dgvUsers.CurrentRow.Cells[0].Value + "]",
               "Confirm",
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsUser.Delete((int)dgvUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "The user become inactive ",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                    MessageBox.Show(
                        "User was not become inactive because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
               "Are you sure you want to become user inactive ["
               + dgvUsers.CurrentRow.Cells[0].Value + "]",
               "Confirm",
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsUser.Delete((int)dgvUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "The user become inactive ",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                    MessageBox.Show(
                        "User was not become inactive because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm=new frmAddUpdateUser();
            frm.ShowDialog();
            _RefreshData();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm=new frmAddUpdateUser();
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmAddUpdateUser frm= new frmAddUpdateUser(UserID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmAddUpdateUser frm = new frmAddUpdateUser(UserID);
            frm.ShowDialog();            
            _RefreshData();
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            Form frm = new frmShowUserInfo(UserID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            Form frm = new frmShowUserInfo(UserID);
            frm.ShowDialog();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            Form frm = new frmChangePassword(UserID);
            frm.ShowDialog();
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            Form frm = new frmChangePassword(UserID);
            frm.ShowDialog();
        }
    }
}
