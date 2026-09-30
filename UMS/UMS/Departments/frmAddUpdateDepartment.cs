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

namespace UMS.Departments
{
    public partial class frmAddUpdateDepartment : Form
    {
        private int _DepartmentID;
        private clsDepartment _Department;
        private enum enMode { Add,Update}
        private enMode _Mode = enMode.Add;
        public frmAddUpdateDepartment(int DepartmentID)
        {
            InitializeComponent();
            _DepartmentID = DepartmentID;
            _Mode = enMode.Update;
            lblTitle.Text="Update Department";
            this.Text="Update Department";
        }
        public frmAddUpdateDepartment()
        {
            InitializeComponent();
            _Mode= enMode.Add;
            lblTitle.Text="Add New Department";
            this.Text="Add New Department";
            _Department= new clsDepartment();
        }
        private void _Reset()
        {
            lblDepartmentID.Text="???";
            txtBuilding.Clear();
            txtName.Clear();
            lblTitle.Text="Add/Update Department";
            this.Text="Add/Update Department";
        }
        private void _LoadData()
        {
            if( _Mode== enMode.Update)
            {
                _Department=clsDepartment.Find(_DepartmentID);
                if (_Department==null)
                {
                    MessageBox.Show("Not found","Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                _Department.DepartmentID= _DepartmentID;
                lblDepartmentID.Text=_DepartmentID.ToString();
                txtName.Text=_Department.Name;
                txtBuilding.Text=_Department.Building;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmAddUpdateDepartment_Load(object sender, EventArgs e)
        {
            if (_Mode== enMode.Update)
                _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Length==0||txtBuilding.Text.Length==0)
            {
                MessageBox.Show("Fill Records","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _Department.DepartmentID=_DepartmentID;
            _Department.Name= txtName.Text.Trim();
            _Department.Building= txtBuilding.Text.Trim();
            if (_Department.Save())
            {
                if (_Mode == enMode.Add)
                {
                    _DepartmentID = _Department.DepartmentID;

                    lblDepartmentID.Text = _DepartmentID.ToString();

                    MessageBox.Show(
                        $"Department Added [{_DepartmentID}] Successfully",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _Mode = enMode.Update;
                    lblTitle.Text = "Update Person";
                }
                else
                {
                    MessageBox.Show(
                        "Department Updated Successfully",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(
                    "Operation Failed",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
