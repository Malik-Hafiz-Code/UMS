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
    public partial class frmShowDoctorInfo : Form
    {
        private int _DoctorID;
        public frmShowDoctorInfo(int DoctorID)
        {
            InitializeComponent();
            _DoctorID=DoctorID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmShowDoctorInfo_Load(object sender, EventArgs e)
        {
            ctrlDoctorInfo1.LoadData(_DoctorID);
        }
    }
}
