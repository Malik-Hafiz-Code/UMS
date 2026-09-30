using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UMS.People
{
    public partial class frmShowPersonInfo : Form
    {
        private int _PersonID;
        public frmShowPersonInfo(int personID)
        {
            InitializeComponent();
            _PersonID=personID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowPersonInfo_Load(object sender, EventArgs e)
        {
            ctrlPersonInfo1.LoadPersonInfo(_PersonID);
        }
    }
}
