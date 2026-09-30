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

namespace UMS.Doctors
{
    public partial class frmListDoctors : Form
    {
        private DataTable _dtDoctors = clsDoctor.GetAllDoctors();
        public frmListDoctors()
        {
            InitializeComponent();
        }
        private void _Refresh()
        {
            _dtDoctors=clsDoctor.GetAllDoctors();
            dgvDoctors.DataSource = _dtDoctors;
            lblRecords.Text=dgvDoctors.Rows.Count.ToString();
        }
        private void frmListDoctors_Load(object sender, EventArgs e)
        {
            _Refresh();
            cbFilterBy.SelectedIndex=0;
            if (dgvDoctors.Rows.Count>0)
            {
                dgvDoctors.Columns[0].HeaderText="Doctor ID";
                dgvDoctors.Columns[0].Width=100;
                dgvDoctors.Columns[1].HeaderText="Name";
                dgvDoctors.Columns[1].Width=150;
                dgvDoctors.Columns[2].HeaderText="Person ID";
                dgvDoctors.Columns[2].Width=100;        
                dgvDoctors.Columns[3].HeaderText="Department ID";
                dgvDoctors.Columns[3].Width=150;
                dgvDoctors.Columns[4].HeaderText="Salary";
                dgvDoctors.Columns[4].Width=100;
                dgvDoctors.Columns[5].HeaderText="Is Active";
                dgvDoctors.Columns[5].Width=90;
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
            string FilterColoumn = "";
            switch (cbFilterBy.Text)
            {
                case "DoctorID":
                    FilterColoumn="DoctorID";
                    break;
                case "PersonID":
                    FilterColoumn="PersonID";
                    break;
                case "Name":
                    FilterColoumn="Name";
                    break;
                case "DepartmentID":
                    FilterColoumn="DepartmentID";
                    break;
                case "Salary":
                    FilterColoumn="Salary";
                    break;
                default:
                    FilterColoumn="None";
                    break;
            }
            if (FilterColoumn=="None"||txtFilter.Text.Trim()=="")
            {
                _dtDoctors.DefaultView.RowFilter="";
                lblRecords.Text=dgvDoctors.RowCount.ToString();
                return;
            }
            if (FilterColoumn=="DoctorID"||FilterColoumn=="PersonID"
                ||FilterColoumn=="DepartmentID"||FilterColoumn=="Salary")
                _dtDoctors.DefaultView.RowFilter=string.Format("[{0}]={1}"
                    , FilterColoumn, txtFilter.Text);
            else
                _dtDoctors.DefaultView.RowFilter=string.Format("[{0}] LIKE '{1}%'",
                    FilterColoumn, txtFilter.Text);
            lblRecords.Text=dgvDoctors.RowCount.ToString();
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
                _dtDoctors.DefaultView.RowFilter="";
            else
                _dtDoctors.DefaultView.RowFilter=string.Format(
                    "[{0}]={1}", "Status", FilterValue);
            lblRecords.Text=dgvDoctors.RowCount.ToString();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
               "Are you sure you want to delete Doctor ["
               + dgvDoctors.CurrentRow.Cells[0].Value + "]",
               "Confirm Delete",
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsDoctor.Delete((int)dgvDoctors.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Doctor Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _Refresh();
                }
                else
                {
                    MessageBox.Show(
                        "Doctor was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
               "Are you sure you want to delete Doctor ["
               + dgvDoctors.CurrentRow.Cells[0].Value + "]",
               "Confirm Delete",
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsDoctor.Delete((int)dgvDoctors.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Doctor Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _Refresh();
                }
                else
                {
                    MessageBox.Show(
                        "Doctor was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateDoctor frm = new frmAddUpdateDoctor();
            frm.ShowDialog();
            _Refresh();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateDoctor frm = new frmAddUpdateDoctor();
            frm.ShowDialog();
            _Refresh();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int DoctorID = (int)dgvDoctors.CurrentRow.Cells[0].Value;
            frmAddUpdateDoctor frm = new frmAddUpdateDoctor(DoctorID);
            frm.ShowDialog();
            _Refresh();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DoctorID = (int)dgvDoctors.CurrentRow.Cells[0].Value;
            frmAddUpdateDoctor frm = new frmAddUpdateDoctor(DoctorID);
            frm.ShowDialog();
            _Refresh();
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            int DoctorID = (int)dgvDoctors.CurrentRow.Cells[0].Value;
            frmShowDoctorInfo frm = new frmShowDoctorInfo(DoctorID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DoctorID = (int)dgvDoctors.CurrentRow.Cells[0].Value;
            frmShowDoctorInfo frm = new frmShowDoctorInfo(DoctorID);
            frm.ShowDialog();
        }
    }
}
