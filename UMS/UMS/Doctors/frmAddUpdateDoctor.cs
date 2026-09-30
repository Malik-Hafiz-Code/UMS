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
    public partial class frmAddUpdateDoctor : Form
    {
        private enum enMode { Add, Update }
        enMode _Mode = enMode.Add;
        private int _DoctorID;
        private clsDoctor _Doctor;
        public frmAddUpdateDoctor()
        {
            InitializeComponent();
            _Mode = enMode.Add;
            _Doctor=new clsDoctor();
            this.Text="Add New Doctor";
            lblTitle.Text="Add New Doctor";
            btnSave.Enabled=false;
            tpDoctorInfo.Enabled=false;
        }
        public frmAddUpdateDoctor(int DoctorID)
        {
            InitializeComponent();
            _DoctorID = DoctorID;
            _Mode= enMode.Update;
            this.Text="Update Doctor";
            lblTitle.Text="Update Doctor";
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
            _Doctor=clsDoctor.Find(_DoctorID);
            if (_Doctor==null)
            {
                MessageBox.Show($"Not found: {_DoctorID}","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlPersonInfoWithFilter1.LoadInfo(_Doctor.PersonID);
            lblDoctorID.Text=_DoctorID.ToString();
            cbDepartment.SelectedValue=_Doctor.Department.DepartmentID;
            txtSalary.Text=_Doctor.Salary.ToString();
            chkIsActive.Checked = _Doctor.IsActive;
        }
        private void frmAddUpdateDoctor_Load(object sender, EventArgs e)
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
            _Doctor.PersonID=ctrlPersonInfoWithFilter1.PersonID;
            _Doctor.PersonInfo=ctrlPersonInfoWithFilter1.SelectedPersonInfo;
            tpDoctorInfo.Enabled=true;
            tcDoctorInfo.SelectedTab=tpDoctorInfo;
            btnSave.Enabled=true;
            ctrlPersonInfoWithFilter1.FilterEnabled=false;
        }
        private bool _Validation()
        {
            if (_Mode == enMode.Add)
            {               
                if (clsDoctor.IsPersonDoctor(_Doctor.PersonID))
                {
                    MessageBox.Show("This Person already has a Doctor account.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (clsStudent.IsPersonStudent(_Doctor.PersonID))
                {
                    MessageBox.Show("This Person is student account.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            int age = DateTime.Now.Year - _Doctor.PersonInfo.DateOfBirth.Year;
            if (DateTime.Now.Date < _Doctor.PersonInfo.DateOfBirth.AddYears(age))
                age--;

            if (age < 25)
            {
                MessageBox.Show("Doctor must be at least 25 years old.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtSalary.Text.Trim() == "")
            {
                MessageBox.Show("Fill Salary", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtSalary.Focus();
                return false;
            }

            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_Validation())
                return;
            int selectedDepartmentID = (int)cbDepartment.SelectedValue;
            _Doctor.Department = clsDepartment.Find(selectedDepartmentID);
            decimal.TryParse(txtSalary.Text.Trim(), out decimal salary);
            _Doctor.Salary = salary;
            _Doctor.IsActive=chkIsActive.Checked;
            if (_Mode == enMode.Add)
            {
                if (_Doctor.Save())
                {
                    _DoctorID = _Doctor.DoctorID;
                    lblDoctorID.Text = _DoctorID.ToString();
                    MessageBox.Show(
                        $"Doctor [{_DoctorID}] added successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    _Mode = enMode.Update;
                    lblTitle.Text = "Update Doctor";
                    this.Text = lblTitle.Text;
                    ctrlPersonInfoWithFilter1.FilterEnabled = false;
                }
                else
                {
                    MessageBox.Show(
                        "Doctor was not added.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                if (_Doctor.Save())
                {
                    MessageBox.Show(
                        "Doctor updated successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Doctor was not updated.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
