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
using UMS.Global;

namespace UMS.Sections
{
    public partial class frmListSections : Form
    {
        private DataTable _dtSection=clsSection.GetAllSections();
        private void _RefreshData()
        {
            _dtSection=clsSection.GetAllSections();
            dgvSections.DataSource= _dtSection;
            lblRecords.Text=dgvSections.RowCount.ToString();
        }
        public frmListSections()
        {
            InitializeComponent();
        }

        private void frmListSections_Load(object sender, EventArgs e)
        {
            _RefreshData();
            bool canModify = clsGlobal.CanModify();
            btnAdd.Visible=canModify;
            btnDelete.Visible=canModify;
            btnEdit.Visible=canModify;
            addNewSectionToolStripMenuItem.Visible=canModify;
            editToolStripMenuItem.Visible=canModify;
            deleteToolStripMenuItem.Visible=canModify;
            cbFilterBy.SelectedIndex=0;
            if(dgvSections.Rows.Count > 0)
            {
                dgvSections.Columns[0].HeaderText="Section ID";
                dgvSections.Columns[0].Width=110;
                dgvSections.Columns[1].HeaderText="Course Name";
                dgvSections.Columns[1].Width=150;
                dgvSections.Columns[2].HeaderText="Doctor Name";
                dgvSections.Columns[2].Width=170;
                dgvSections.Columns[3].HeaderText="Year";
                dgvSections.Columns[3].Width=90;
                dgvSections.Columns[4].HeaderText="Semester";
                dgvSections.Columns[4].Width=90;
                dgvSections.Columns[5].HeaderText="Room";
                dgvSections.Columns[5].Width=90;
            }
        }

        private void benClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible=cbFilterBy.SelectedIndex!=0;
            if (txtFilter.Visible)
            {
                txtFilter.Clear();
                txtFilter.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColoumn = "";
            switch (cbFilterBy.Text)
            {
                case "SectionID":
                    FilterColoumn="SectionID";
                    break;
                case "CourseName":
                    FilterColoumn="CourseName";
                    break;
                case "DoctorName":
                    FilterColoumn="DoctorName";
                    break;
                case "Year":
                    FilterColoumn="Year";
                    break;
                case "Semester":
                    FilterColoumn="Semester";
                    break;
                case "Room":
                    FilterColoumn="Room";
                    break;
                default:
                    FilterColoumn="None";
                    break;
            }
            if (txtFilter.Text.Trim()==""||FilterColoumn=="None")
            {
                _dtSection.DefaultView.RowFilter="";
                lblRecords.Text=_dtSection.Rows.Count.ToString();
                return;
            }
            if (FilterColoumn=="CourseName"||FilterColoumn=="DoctorName"
                ||FilterColoumn=="Semester")
                _dtSection.DefaultView.RowFilter=string.Format("[{0}] LIKE '{1}%'"
                    , FilterColoumn, txtFilter.Text.Trim());
            else
                _dtSection.DefaultView.RowFilter=string.Format("[{0}]={1}",
                    FilterColoumn, txtFilter.Text.Trim());
            lblRecords.Text=dgvSections.RowCount.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to delete section ["
                + dgvSections.CurrentRow.Cells[0].Value + "]",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsSection.Delete((int)dgvSections.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "section Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "section was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
               "Are you sure you want to delete section ["
               + dgvSections.CurrentRow.Cells[0].Value + "]",
               "Confirm Delete",
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsSection.Delete((int)dgvSections.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "section Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "section was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateSection frm=new frmAddUpdateSection();
            frm.ShowDialog();
            _RefreshData();
        }

        private void addNewDepartmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateSection frm= new frmAddUpdateSection();
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int SectionID = (int)dgvSections.CurrentRow.Cells[0].Value;
            frmAddUpdateSection frm = new frmAddUpdateSection(SectionID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SectionID = (int)dgvSections.CurrentRow.Cells[0].Value;
            frmAddUpdateSection frm = new frmAddUpdateSection(SectionID);
            frm.ShowDialog();
            _RefreshData();
        }
    }
}
