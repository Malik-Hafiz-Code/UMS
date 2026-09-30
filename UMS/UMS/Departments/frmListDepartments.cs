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
using UMS.Global;

namespace UMS.Departments
{
    public partial class frmListDepartments : Form
    {
        private DataTable _dtDepartments = clsDepartment.GetAllDepartments();
        private void _RefreshData()
        {
            _dtDepartments=clsDepartment.GetAllDepartments();
            dgvDepartments.DataSource = _dtDepartments;
            lblRecords.Text=dgvDepartments.RowCount.ToString();
        }
        public frmListDepartments()
        {
            InitializeComponent();
        }

        private void benClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmListDepartments_Load(object sender, EventArgs e)
        {
            _RefreshData();
            bool canModify = clsGlobal.CanModify();
            btnAdd.Visible=canModify;
            btnDelete.Visible=canModify;
            btnEdit.Visible=canModify;
            addNewDepartmentToolStripMenuItem.Visible=canModify;
            editToolStripMenuItem.Visible=canModify;
            deleteToolStripMenuItem.Visible=canModify;
            cbFilterBy.SelectedIndex = 0;
            if (dgvDepartments.Rows.Count>0)
            {
                dgvDepartments.Columns[0].HeaderText="Department ID";
                dgvDepartments.Columns[0].Width= 140;
                dgvDepartments.Columns[1].HeaderText="Name";
                dgvDepartments.Columns[1].Width=195;
                dgvDepartments.Columns[2].HeaderText="Building";
                dgvDepartments.Columns[2].Width= 120;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "DepartmentID":
                    FilterColumn="DepartmentID";
                    break;
                case "Name":
                    FilterColumn = "Name";
                    break;
                case "Building":
                    FilterColumn ="Building";
                    break;
                default:
                    FilterColumn="None";
                    break;
            }
            if (txtFilter.Text.Trim()==""||FilterColumn=="None")
            {
                _dtDepartments.DefaultView.RowFilter="";
                lblRecords.Text=dgvDepartments.RowCount.ToString();
                return;
            }
            if (FilterColumn=="DepartmentID")
            {
                _dtDepartments.DefaultView.RowFilter=string.Format(
                    "[{0}]={1}", FilterColumn, txtFilter.Text.Trim());
            }
            else
            {
                _dtDepartments.DefaultView.RowFilter =
                    string.Format(
                        "[{0}] LIKE '{1}%'",
                        FilterColumn,
                        txtFilter.Text.Trim());
            }

            lblRecords.Text = dgvDepartments.Rows.Count.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible=(cbFilterBy.SelectedIndex!=0);
            if (txtFilter.Visible)
            {
                txtFilter.Text="";
                txtFilter.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to delete department ["
                + dgvDepartments.CurrentRow.Cells[0].Value + "]",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsDepartment.Delete((int)dgvDepartments.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Department Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "Department was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
               "Are you sure you want to delete department ["
               + dgvDepartments.CurrentRow.Cells[0].Value + "]",
               "Confirm Delete",
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsPerson.Delete((int)dgvDepartments.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Department Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "department was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateDepartment frm= new frmAddUpdateDepartment();
            frm.ShowDialog();
            _RefreshData();
        }

        private void addNewDepartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateDepartment frm = new frmAddUpdateDepartment();
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int DepartmentID = (int)dgvDepartments.CurrentRow.Cells[0].Value;
            frmAddUpdateDepartment frm = new frmAddUpdateDepartment(DepartmentID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DepartmentID = (int)dgvDepartments.CurrentRow.Cells[0].Value;
            frmAddUpdateDepartment frm = new frmAddUpdateDepartment(DepartmentID);
            frm.ShowDialog();
            _RefreshData();
        }
    }
}
