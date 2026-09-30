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
using static System.Collections.Specialized.BitVector32;

namespace UMS.Enrollments
{
    public partial class frmAddUpdateEnrollment : Form
    {
        private int _EnrollmentID;
        private clsEnrollment _Enrollment;
        private enum enMode { Add, Update }
        private enMode _Mode = enMode.Add;
        public frmAddUpdateEnrollment()
        {
            InitializeComponent();
            _Mode=enMode.Add;
            lblTitle.Text="Add New Enrollment";
            this.Text=lblTitle.Text;
            _Enrollment=new clsEnrollment();
        }
        public frmAddUpdateEnrollment(int EnrollmentID)
        {
            InitializeComponent();
            _EnrollmentID = EnrollmentID;
            _Mode=enMode.Update;
            lblTitle.Text="Update Enrollment";
            this.Text=lblTitle.Text;
        }
        private void _LoadStudentName()
        {
            DataTable dt=clsStudent.GetAllStudents();
            cbStudentName.DataSource=dt;
            cbStudentName.DisplayMember="Name";
            cbStudentName.ValueMember="StudentID";
        }
        private void _LoadSections()
        {
            DataTable dt = clsSection.GetAllSectionsRaw();

            dt.Columns.Add("SectionDisplay", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                int courseID = (int)row["CourseID"];
                clsCourse course = clsCourse.Find(courseID);

                row["SectionDisplay"] = $"{course.Name} (#{row["SectionID"]})";
            }

            cbSectionName.DataSource = dt;
            cbSectionName.DisplayMember = "SectionDisplay";
            cbSectionName.ValueMember = "SectionID";
        }
        private void _LoadData()
        {
            _Enrollment=clsEnrollment.Find(_EnrollmentID);
            if( _Enrollment==null)
            {
                MessageBox.Show($"Not Found {_EnrollmentID}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblEnrollmentID.Text = _EnrollmentID.ToString();
            cbSectionName.SelectedValue = _Enrollment.Section.SectionID;
            cbStatus.Text = _Enrollment.Status.ToString();
            txtGrade.Text=_Enrollment.Grade.ToString();
            cbStudentName.SelectedValue=_Enrollment.Student.StudentID;
            dtpEnrollmentDate.Value=_Enrollment.EnrollmentDate;
        }
        private void frmAddUpdateEnrollment_Load(object sender, EventArgs e)
        {
            dtpEnrollmentDate.MaxDate=DateTime.Now;
            _LoadStudentName();
            _LoadSections();
            if (_Mode==enMode.Update)
                _LoadData();
            else
                cbStatus.SelectedIndex=0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool _Validation()
        {
            if (txtGrade.Text.Trim() == "")
            {
                MessageBox.Show("Fill grade", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!double.TryParse(txtGrade.Text.Trim(), out double grade))
            {
                MessageBox.Show("Grade must be a valid number", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (grade<0||grade>100)
            {
                MessageBox.Show("Grade must be between 0 and 100","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGrade.Focus();
                return false;
            }

            if ((cbStatus.Text == "Active" || cbStatus.Text == "Withdrawn") && grade != 0)
            {
                MessageBox.Show($"Grade must be 0 because the status is {cbStatus.Text}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGrade.Focus();
                return false;
            }

            if (cbStatus.Text=="Completed"&&grade<50)
            {
                MessageBox.Show($"Grade must be greater than or equal 50", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGrade.Focus();
                return false;
            }

            if (cbStatus.Text=="Failed"&&grade>=50)
            {
                MessageBox.Show($"Grade must be slowe than 50", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGrade.Focus();
                return false;
            }

            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_Validation())
                return;
            int studentID =(int)cbStudentName.SelectedValue;
            clsStudent student = clsStudent.Find(studentID);
            _Enrollment.Student=student;
            _Enrollment.Status=(clsEnrollment.enStatus)Enum.Parse
                (typeof(clsEnrollment.enStatus), cbStatus.Text);
            double.TryParse(txtGrade.Text, out double grade);
            _Enrollment.Grade=grade;
            int sectionID =(int) cbSectionName.SelectedValue;
            _Enrollment.Section=clsSection.Find(sectionID);
            _Enrollment.EnrollmentDate=dtpEnrollmentDate.Value;
            if (_Mode == enMode.Add)
            {
                if (_Enrollment.Save())
                {
                    _EnrollmentID = _Enrollment.EnrollmentID;
                    lblEnrollmentID.Text = _EnrollmentID.ToString();
                    MessageBox.Show(
                        $"Enrollment [{_EnrollmentID}] added successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    _Mode = enMode.Update;
                    lblTitle.Text = "Update Enrollment";
                    this.Text = lblTitle.Text;
                }
                else
                {
                    MessageBox.Show(
                        "Enrollment was not added.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                if (_Enrollment.Save())
                {
                    MessageBox.Show(
                        "Enrollment updated successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Enrollment was not updated.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
