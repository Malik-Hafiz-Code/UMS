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
    public partial class ctrlStudentInfo : UserControl
    {
        private int _StudentID;
        private clsStudent _Student;
        public ctrlStudentInfo()
        {
            InitializeComponent();
        }
        public void Reset()
        {
            ctrlPersonInfo1.Reset();
            lblDepartmentName.Text="???";
            lblStudentID.Text="???";
            lblGPA.Text="???";
            lblStatus.Text="???";
            _StudentID=-1;
            _Student= null;
        }
        public int StudentID { get { return _StudentID; } }
        public clsStudent Student {  get { return _Student; } }
        public void LoadData(int StudentID)
        {
            _Student=clsStudent.Find(StudentID);
            if( _Student==null)
            {
                MessageBox.Show($"Not found {StudentID}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Reset();
                return;
            }
            _StudentID = StudentID;
            ctrlPersonInfo1.LoadPersonInfo(_Student.PersonID);
            lblStudentID.Text=_StudentID.ToString();
            lblEnrollmentDate.Text=_Student.EnrollmentDate.ToShortDateString();
            lblDepartmentName.Text=_Student.Department.Name;
            lblStatus.Text=_Student.Status.ToString();
            lblGPA.Text=_Student.GPA.ToString();
        }
    }
}
