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
    public partial class frmShowStudentInfo : Form
    {
        private int _StudentID;
        public frmShowStudentInfo(int StudentID)
        {
            InitializeComponent();
            _StudentID=StudentID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmShowStudentInfo_Load(object sender, EventArgs e)
        {
            ctrlStudentInfo1.LoadData(_StudentID);
        }
    }
}
