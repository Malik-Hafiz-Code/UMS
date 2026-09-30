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
using UMS.People.Controls;

namespace UMS.Courses
{
    public partial class frmAddUpdateCourse : Form
    {
        private int _CourseID;
        private clsCourse _Course;
        private enum enMode { Add,Update}
        private enMode _Mode=enMode.Add;
        public frmAddUpdateCourse()
        {
            InitializeComponent();
            _Mode=enMode.Add;
            lblTitle.Text="Add New Course";
            this.Text=lblTitle.Text;
            _Course=new clsCourse();
        }
        public frmAddUpdateCourse(int CourseID)
        {
            InitializeComponent();
            _CourseID = CourseID;
            _Mode = enMode.Update;
            lblTitle.Text="Update Course";
            this.Text=lblTitle.Text;
        }
        private void _LoadDepartments()
        {
            DataTable dt=clsDepartment.GetAllDepartments();
            cbDepartmentName.DataSource=dt;
            cbDepartmentName.DisplayMember = "Name";
            cbDepartmentName.ValueMember = "DepartmentID";
        }
        private void _LoadData()
        {
            _Course=clsCourse.Find(_CourseID);
            if (_Course==null)
            {
                MessageBox.Show($"Not found: {_CourseID}","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblCourseID.Text=_CourseID.ToString();
            txtCourseName.Text=_Course.Name;
            cbDepartmentName.SelectedValue=_Course.Department.DepartmentID;
            numericUpDown1.Value=_Course.Credit;
            cbAcademicLevel.Text=_Course.AcademicLevel.ToString();
        }
        private void frmAddUpdateCourse_Load(object sender, EventArgs e)
        {
            _LoadDepartments();
            if (_Mode==enMode.Update)
                _LoadData();
            else
                cbAcademicLevel.SelectedIndex=0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCourseName.Text.Trim()=="")
            {
                MessageBox.Show("Fill Course Name","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int selectedDepartmentID=(int)cbDepartmentName.SelectedValue;
            _Course.Department=clsDepartment.Find(selectedDepartmentID);
            _Course.Name=txtCourseName.Text;
            _Course.Credit=Convert.ToInt16(numericUpDown1.Value);
            _Course.AcademicLevel=Convert.ToInt16(cbAcademicLevel.Text);
            if (_Mode == enMode.Add)
            {
                if (_Course.Save())
                {
                    _CourseID = _Course.CourseID;
                    lblCourseID.Text = _CourseID.ToString();
                    MessageBox.Show(
                        $"Course [{_CourseID}] added successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    _Mode = enMode.Update;
                    lblTitle.Text = "Update Course";
                    this.Text = lblTitle.Text;
                }
                else
                {
                    MessageBox.Show(
                        "Course was not added.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                if (_Course.Save())
                {
                    MessageBox.Show(
                        "Course updated successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Course was not updated.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
