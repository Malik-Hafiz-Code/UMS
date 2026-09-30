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

namespace UMS.Home
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            lblPeople.Text=clsPerson.CountPeople.ToString();
            lblAllUsers.Text=clsUser.CountAllUsers.ToString();
            lblActiveUsers.Text=clsUser.CountActiveUsers.ToString();
            lblStudents.Text=clsStudent.CountStudents.ToString();
            lblDoctors.Text=clsDoctor.CountDoctors.ToString();
            lblDepartments.Text=clsDepartment.CountDepartments.ToString();
        }

        private void lblActiveUsers_Click(object sender, EventArgs e)
        {

        }

        private void lblAllUsers_Click(object sender, EventArgs e)
        {

        }

        private void lblPeople_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
