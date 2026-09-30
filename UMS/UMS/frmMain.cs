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
using UMS.Courses;
using UMS.Departments;
using UMS.Doctors;
using UMS.Enrollments;
using UMS.Global;
using UMS.Home;
using UMS.People;
using UMS.Sections;
using UMS.Students;
using UMS.Users;

namespace UMS
{
    public partial class frmMain : Form
    {
        frmLogin _frmLogin;
        public frmMain(frmLogin frm)
        {
            InitializeComponent();
            _frmLogin = frm;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblTime.Text=DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            lblWelcome.Text=$"Welcome: {clsGlobal.Username}";
            switch (clsGlobal.CurrentUser.Role)
            {
                case clsUser.enRole.Student:
                    btnStudents.Enabled = false;
                    btnEnrollments.Enabled = false;
                    btnDoctors.Enabled = false;
                    btnPeople.Enabled = false;
                    btnUsers.Enabled=false;
                    break;
                case clsUser.enRole.Doctor:
                    btnStudents.Enabled = false;
                    btnUsers.Enabled=false;
                    btnDoctors.Enabled=false;
                    btnEnrollments.Enabled=false;
                    break;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmHome frm=new frmHome();
            frm.ShowDialog();
        }

        private void btnPeople_Click(object sender, EventArgs e)
        {
            frmListPeople frm = new frmListPeople();
            frm.ShowDialog();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            frmListUser frm = new frmListUser();
            frm.ShowDialog();
        }

        private void btnDepartments_Click(object sender, EventArgs e)
        {
            frmListDepartments frm= new frmListDepartments();
            frm.ShowDialog();
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            frmListDoctors frm = new frmListDoctors();
            frm.ShowDialog();
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            frmListStudents frm=new frmListStudents();
            frm.ShowDialog();
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            frmListCourses frm=new frmListCourses();
            frm.ShowDialog();
        }

        private void btnSections_Click(object sender, EventArgs e)
        {
            frmListSections frm=new frmListSections();
            frm.ShowDialog();
        }

        private void btnEnrollments_Click(object sender, EventArgs e)
        {
            frmListEnrollments frm=new frmListEnrollments();
            frm.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser=null;
            _frmLogin.Show();
            this.Close();
        }
    }
}
