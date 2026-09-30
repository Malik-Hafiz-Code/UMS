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

namespace UMS.Enrollments
{
    public partial class frmListEnrollments : Form
    {
        private DataTable _dtEnrollments=clsEnrollment.GetAllEnrollments();
        private void _RefreshData()
        {
            _dtEnrollments=clsEnrollment.GetAllEnrollments();
            dgvEnrollments.DataSource = _dtEnrollments;
            lblRecords.Text=dgvEnrollments.RowCount.ToString();
        }
        public frmListEnrollments()
        {
            InitializeComponent();
        }

        private void fmrListEnrollments_Load(object sender, EventArgs e)
        {
            _RefreshData();
            bool canModify = clsGlobal.CanModify();
            btnAdd.Visible=canModify;
            btnDelete.Visible=canModify;
            btnEdit.Visible=canModify;
            addNewEnrollmentToolStripMenuItem.Visible=canModify;
            editToolStripMenuItem.Visible=canModify;
            deleteToolStripMenuItem.Visible=canModify;
            cbFilterBy.SelectedIndex= 0;
            if(dgvEnrollments.Rows.Count > 0)
            {
                dgvEnrollments.Columns[0].HeaderText="Enrollment ID";
                dgvEnrollments.Columns[0].Width=130;
                dgvEnrollments.Columns[1].HeaderText="Student Name";
                dgvEnrollments.Columns[1].Width=150;
                dgvEnrollments.Columns[2].HeaderText="Section ID";
                dgvEnrollments.Columns[2].Width=110;
                dgvEnrollments.Columns[3].HeaderText="Grade";
                dgvEnrollments.Columns[3].Width=100;
                dgvEnrollments.Columns[4].HeaderText="Status";
                dgvEnrollments.Columns[4].Width=130;
                dgvEnrollments.Columns[5].HeaderText="Enrollment Date";
                dgvEnrollments.Columns[5].Width=140;
            }
        }

        private void benClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible=cbFilterBy.SelectedIndex!=0;
            if (txtFilter.Visible)
            {
                txtFilter.Clear();
                txtFilter.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColoumn = "";
            switch (cbFilterBy.Text)
            {
                case "EnrollmentID":
                    FilterColoumn="EnrollmentID";
                    break;
                case "StudentName":
                    FilterColoumn="StudentName";
                    break;
                case "SectionID":
                    FilterColoumn="SectionID";
                    break;
                case "Grade":
                    FilterColoumn="Grade";
                    break;
                case "Status":
                    FilterColoumn="Status";
                    break;
                default:
                    FilterColoumn="None";
                    break;
            }
            if (txtFilter.Text.Trim()==""||FilterColoumn=="None")
            {
                _dtEnrollments.DefaultView.RowFilter="";
                lblRecords.Text=dgvEnrollments.RowCount.ToString();
                return;
            }
            if (FilterColoumn=="StudentName"||FilterColoumn=="Status")
                _dtEnrollments.DefaultView.RowFilter=string.Format(
                    "[{0}] LIKE '{1}%'", FilterColoumn, txtFilter.Text.Trim());
            else
                _dtEnrollments.DefaultView.RowFilter=string.Format(
                    "[{0}]={1}", FilterColoumn, txtFilter.Text.Trim());
            lblRecords.Text=dgvEnrollments.RowCount.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
               "Are you sure you want to delete Enrollment ["
               + dgvEnrollments.CurrentRow.Cells[0].Value + "]",
               "Confirm Delete",
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsEnrollment.Delete((int)dgvEnrollments.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Enrollment Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "Enrollment was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
              "Are you sure you want to delete Enrollment ["
              + dgvEnrollments.CurrentRow.Cells[0].Value + "]",
              "Confirm Delete",
              MessageBoxButtons.OKCancel,
              MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsEnrollment.Delete((int)dgvEnrollments.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Enrollment Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "Enrollment was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateEnrollment frm= new frmAddUpdateEnrollment();
            frm.ShowDialog();
            _RefreshData();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateEnrollment frm=new frmAddUpdateEnrollment();
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int EnrollmentID = (int)dgvEnrollments.CurrentRow.Cells[0].Value;
            frmAddUpdateEnrollment frm = new frmAddUpdateEnrollment(EnrollmentID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int EnrollmentID = (int)dgvEnrollments.CurrentRow.Cells[0].Value;
            frmAddUpdateEnrollment frm = new frmAddUpdateEnrollment(EnrollmentID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            int EnrollmentID = (int)dgvEnrollments.CurrentRow.Cells[0].Value;
            frmShowEnrollmentInfo frm=new frmShowEnrollmentInfo(EnrollmentID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int EnrollmentID = (int)dgvEnrollments.CurrentRow.Cells[0].Value;
            frmShowEnrollmentInfo frm = new frmShowEnrollmentInfo(EnrollmentID);
            frm.ShowDialog();
        }
    }
}
