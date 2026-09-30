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
    public partial class ctrlDoctorInfo : UserControl
    {
        private int _DoctorID;
        private clsDoctor _Doctor;
        public ctrlDoctorInfo()
        {
            InitializeComponent();
        }
        public int DoctorID {  get { return _DoctorID; }}
        public clsDoctor Doctor { get { return _Doctor; }}
        public void Reset()
        {
            ctrlPersonInfo1.Reset();
            lblDepartmentName.Text="???";
            lblDoctorID.Text="???";
            lblSalary.Text="???";
            lblIsActive.Text="???";
            _DoctorID=-1;
            _Doctor= null;
        }
        public void LoadData(int DoctorID)
        {
            Reset();
            _Doctor=clsDoctor.Find(DoctorID);
            if (_Doctor==null)
            {
                MessageBox.Show($"Not found: {DoctorID}","Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                Reset();
                return;
            }
            _DoctorID=DoctorID;
            ctrlPersonInfo1.LoadPersonInfo(_Doctor.PersonID);
            lblDoctorID.Text = _DoctorID.ToString();
            lblSalary.Text=_Doctor.Salary.ToString();
            lblDepartmentName.Text=_Doctor.Department.Name;
            lblIsActive.Text=_Doctor.IsActive ? "True" : "False";
        }
    }
}
