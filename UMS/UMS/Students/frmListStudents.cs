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

namespace UMS.Students
{
    public partial class frmListStudents : Form
    {
        private DataTable _dtStudents=clsStudent.GetAllStudents();
        private void _Refresh()
        {
            _dtStudents = clsStudent.GetAllStudents();
            dgvStudents.DataSource = _dtStudents;
            lblRecords.Text=dgvStudents.RowCount.ToString();
        }
        public frmListStudents()
        {
            InitializeComponent();
        }

        private void benClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmListStudents_Load(object sender, EventArgs e)
        {
            _Refresh();
            cbFilterBy.SelectedIndex=0;
            if (dgvStudents.Rows.Count>0)
            {
                dgvStudents.Columns[0].HeaderText="Student ID";
                dgvStudents.Columns[0].Width=90;
                dgvStudents.Columns[1].HeaderText="Name";
                dgvStudents.Columns[1].Width=150;
                dgvStudents.Columns[2].HeaderText="Person ID";
                dgvStudents.Columns[2].Width=90;
                dgvStudents.Columns[3].HeaderText="Department ID";
                dgvStudents.Columns[3].Width=130;
                dgvStudents.Columns[4].HeaderText="Enrollment Date";
                dgvStudents.Columns[4].Width=150;
                dgvStudents.Columns[5].HeaderText="Status";
                dgvStudents.Columns[5].Width=90;
                dgvStudents.Columns[6].HeaderText="GPA";
                dgvStudents.Columns[6].Width=90;
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible=cbFilterBy.SelectedIndex!=0;
            if(txtFilter.Visible)
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
                case "StudentID":
                    FilterColoumn="StudentID";
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
                case "Status":
                    FilterColoumn="Status";
                    break;
                case "GPA":
                    FilterColoumn="GPA";
                    break;
                default:
                    FilterColoumn="None";
                    break;
            }
            if (FilterColoumn=="None"||txtFilter.Text.Trim()=="")
            {
                _dtStudents.DefaultView.RowFilter="";
                lblRecords.Text=dgvStudents.RowCount.ToString();
                return;
            }
            if (FilterColoumn=="StudentID"||FilterColoumn=="PersonID"
                ||FilterColoumn=="DepartmentID"||FilterColoumn=="GPA")
                _dtStudents.DefaultView.RowFilter=string.Format("[{0}]={1}"
                    , FilterColoumn, txtFilter.Text);
            else
                _dtStudents.DefaultView.RowFilter=string.Format("[{0}] LIKE '{1}%'",
                    FilterColoumn, txtFilter.Text);
            lblRecords.Text=dgvStudents.RowCount.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
              "Are you sure you want to delete Student ["
              + dgvStudents.CurrentRow.Cells[0].Value + "]",
              "Confirm Delete",
              MessageBoxButtons.OKCancel,
              MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsStudent.Delete((int)dgvStudents.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Student Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _Refresh();
                }
                else
                {
                    MessageBox.Show(
                        "Student was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
              "Are you sure you want to delete Student ["
              + dgvStudents.CurrentRow.Cells[0].Value + "]",
              "Confirm Delete",
              MessageBoxButtons.OKCancel,
              MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsDoctor.Delete((int)dgvStudents.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Student Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _Refresh();
                }
                else
                {
                    MessageBox.Show(
                        "Student was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateStudent frm=new frmAddUpdateStudent();
            frm.ShowDialog();
            _Refresh();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateStudent frm = new frmAddUpdateStudent();
            frm.ShowDialog();
            _Refresh();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int StudentID = (int)dgvStudents.CurrentRow.Cells[0].Value;
            frmAddUpdateStudent frm = new frmAddUpdateStudent(StudentID);
            frm.ShowDialog();
            _Refresh();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int StudentID = (int)dgvStudents.CurrentRow.Cells[0].Value;
            frmAddUpdateStudent frm = new frmAddUpdateStudent(StudentID);
            frm.ShowDialog();
            _Refresh();
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            int StudentID = (int)dgvStudents.CurrentRow.Cells[0].Value;
            frmShowStudentInfo frm = new frmShowStudentInfo(StudentID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int StudentID = (int)dgvStudents.CurrentRow.Cells[0].Value;
            frmShowStudentInfo frm = new frmShowStudentInfo(StudentID);
            frm.ShowDialog();
        }
    }
}
