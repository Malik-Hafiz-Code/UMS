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

namespace UMS.Courses
{
    public partial class frmListCourses : Form
    {
        private DataTable _dtCourses=clsCourse.GetAllCourses();
        private void _RefreshData()
        {
            _dtCourses = clsCourse.GetAllCourses();
            dgvCourses.DataSource = _dtCourses;
            lblRecords.Text=dgvCourses.RowCount.ToString();
        }
        public frmListCourses()
        {
            InitializeComponent();
        }

        private void frmListCourses_Load(object sender, EventArgs e)
        {
            _RefreshData();
            bool canModify = clsGlobal.CanModify();
            btnAdd.Visible=canModify;
            btnDelete.Visible=canModify;
            btnEdit.Visible=canModify;
            addNewCourseToolStripMenuItem.Visible=canModify;
            editToolStripMenuItem.Visible=canModify;
            deleteToolStripMenuItem.Visible=canModify;
            cbFilterBy.SelectedIndex=0;
            if (dgvCourses.RowCount>0)
            {
                dgvCourses.Columns[0].HeaderText="Course ID";
                dgvCourses.Columns[0].Width=110;
                dgvCourses.Columns[1].HeaderText="Department ID";
                dgvCourses.Columns[1].Width=150;
                dgvCourses.Columns[2].HeaderText="Name";
                dgvCourses.Columns[2].Width=150;
                dgvCourses.Columns[3].HeaderText="Credit";
                dgvCourses.Columns[3].Width=100;
                dgvCourses.Columns[4].HeaderText="Academic Level";
                dgvCourses.Columns[4].Width=160;
            }
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
                case "CourseID":
                    FilterColoumn="CourseID";
                    break;
                case "DepartmentID":
                    FilterColoumn="DepartmentID";
                    break;
                case "Name":
                    FilterColoumn="Name";
                    break;
                case "Credit":
                    FilterColoumn="Credit";
                    break;
                case "AcademicLevel":
                    FilterColoumn="AcademicLevel";
                    break;
                default:
                    FilterColoumn="None";
                    break;
            }
            if (txtFilter.Text.Trim()==""||FilterColoumn=="None")
            {
                _dtCourses.DefaultView.RowFilter="";
                lblRecords.Text=dgvCourses.Rows.Count.ToString();
                return;
            }
            if (FilterColoumn=="Name")
                _dtCourses.DefaultView.RowFilter=string.Format("[{0}] LIKE '{1}%'"
                    , FilterColoumn, txtFilter.Text.Trim());
            else
                _dtCourses.DefaultView.RowFilter=string.Format("[{0}]={1}",
                    FilterColoumn,txtFilter.Text.Trim());
            lblRecords.Text=dgvCourses.RowCount.ToString();
        }

        private void benClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to delete course ["
                + dgvCourses.CurrentRow.Cells[0].Value + "]",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsCourse.Delete((int)dgvCourses.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Course Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "Course was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateCourse frm = new frmAddUpdateCourse();
            frm.ShowDialog();
            _RefreshData();
        }

        private void addNewDepartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateCourse frm= new frmAddUpdateCourse();
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int CourseID = (int)dgvCourses.CurrentRow.Cells[0].Value;
            frmAddUpdateCourse frm = new frmAddUpdateCourse(CourseID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int CourseID = (int)dgvCourses.CurrentRow.Cells[0].Value;
            frmAddUpdateCourse frm = new frmAddUpdateCourse(CourseID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
               "Are you sure you want to delete course ["
               + dgvCourses.CurrentRow.Cells[0].Value + "]",
               "Confirm Delete",
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsCourse.Delete((int)dgvCourses.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Course Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "Course was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
