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
    public partial class frmAddUpdateStudent : Form
    {
        private enum enMode { Add, Update }
        enMode _Mode = enMode.Add;
        private int _StudentID;
        private clsStudent _Student;
        public frmAddUpdateStudent()
        {
            InitializeComponent();
            _Mode = enMode.Add;
            _Student=new clsStudent();
            this.Text="Add New Student";
            lblTitle.Text="Add New Student";
            btnSave.Enabled=false;
            tpStudentInfo.Enabled=false;
            cbStatus.SelectedIndex=0;
        }
        public frmAddUpdateStudent(int StudentID)
        {
            InitializeComponent();
            _StudentID = StudentID;
            _Mode= enMode.Update;
            this.Text="Update Student";
            lblTitle.Text="Update Student";
            ctrlPersonInfoWithFilter1.FilterEnabled=false;
        }
        private void _LoadDepartments()
        {
            DataTable dt = clsDepartment.GetAllDepartments();
            cbDepartment.DataSource = dt;
            cbDepartment.DisplayMember = "Name";
            cbDepartment.ValueMember = "DepartmentID";
        }
        private void _LoadData()
        {
            _Student=clsStudent.Find(_StudentID);
            if (_Student==null)
            {
                MessageBox.Show($"Not found: {_StudentID}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlPersonInfoWithFilter1.LoadInfo(_Student.PersonID);
            lblStudentID.Text=_StudentID.ToString();
            cbDepartment.SelectedValue=_Student.Department.DepartmentID;
            txtGPA.Text=_Student.GPA.ToString();
            cbStatus.Text=_Student.Status.ToString();
        }
        private void frmAddUpdateStudent_Load(object sender, EventArgs e)
        {
            _LoadDepartments();
            if (_Mode== enMode.Update)
                _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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
            _Student.PersonID=ctrlPersonInfoWithFilter1.PersonID;
            _Student.PersonInfo=ctrlPersonInfoWithFilter1.SelectedPersonInfo;
            tpStudentInfo.Enabled=true;
            tcStudentInfo.SelectedTab=tpStudentInfo;
            btnSave.Enabled=true;
            ctrlPersonInfoWithFilter1.FilterEnabled=false;
        }
        private bool _Validation()
        {
            if (_Mode == enMode.Add)
            {           
                if (clsStudent.IsPersonStudent(_Student.PersonID))
                {
                    MessageBox.Show("This Person already has a Student account.", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);               
                    return false;
                }
                if (clsDoctor.IsPersonDoctor(_Student.PersonID))
                {
                    MessageBox.Show("This Person is Doctor account.", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
           
            if (txtGPA.Text.Trim() == "")
            {
                MessageBox.Show("Fill GPA", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtGPA.Focus();
                return false;
            }
            if (Convert.ToDecimal(txtGPA.Text)<0||Convert.ToDecimal(txtGPA.Text)>4)
            {
                MessageBox.Show("GPA must be between 0 and 4", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_Validation())
                return;
            int selectedDepartmentID = (int)cbDepartment.SelectedValue;
            _Student.Department = clsDepartment.Find(selectedDepartmentID);
            decimal.TryParse(txtGPA.Text.Trim(), out decimal GPA);
            _Student.GPA = GPA;
            _Student.Status=(clsStudent.enStatus)
                Enum.Parse(typeof(clsStudent.enStatus), cbStatus.Text);
            if (_Mode == enMode.Add)
            {
                if (_Student.Save())
                {
                    _StudentID = _Student.StudentID;
                    lblStudentID.Text = _StudentID.ToString();
                    MessageBox.Show(
                        $"Student [{_StudentID}] added successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    _Mode = enMode.Update;
                    lblTitle.Text = "Update Student";
                    this.Text = lblTitle.Text;
                    ctrlPersonInfoWithFilter1.FilterEnabled = false;
                }
                else
                {
                    MessageBox.Show(
                        "Student was not added.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                if (_Student.Save())
                {
                    MessageBox.Show(
                        "Student updated successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Student was not updated.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
