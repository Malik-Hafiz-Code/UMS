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
    public partial class frmFindPerson : Form
    {
        private int _PersonID;
        public frmFindPerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmFindPerson_Load(object sender, EventArgs e)
        {
            ctrlPersonInfoWithFilter1.LoadInfo(_PersonID);
        }
    }
}
