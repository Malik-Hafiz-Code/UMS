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

namespace UMS.Enrollments
{
    public partial class frmShowEnrollmentInfo : Form
    {
        private int _EnrollmentID;
        private clsEnrollment _Enrollment;
        public frmShowEnrollmentInfo(int EnrollmentID)
        {
            InitializeComponent();
            _EnrollmentID=EnrollmentID;
        }

        private void frmShowEnrollmentInfo_Load(object sender, EventArgs e)
        {
            _Enrollment=clsEnrollment.Find(_EnrollmentID);
            if (_Enrollment==null)
            {
                MessageBox.Show($"Not Found {_EnrollmentID}","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblEnrollmentID.Text = _EnrollmentID.ToString();
            lblStudnetName.Text=_Enrollment.Student.PersonInfo.Name;
            lblCourseName.Text=_Enrollment.Section.Course.Name;
            lblDoctorName.Text=_Enrollment.Section.Doctor.PersonInfo.Name;
            lblSemetser.Text=_Enrollment.Section.Semester.ToString();
            lblGrade.Text=_Enrollment.Grade.ToString()+" %";
            lblStatus.Text=_Enrollment.Status.ToString();
            lblEnrollmentDate.Text=_Enrollment.EnrollmentDate.ToShortDateString();
            lblRoom.Text=_Enrollment.Section.Room.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
