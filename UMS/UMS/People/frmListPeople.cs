using Business;
using System;
using System.Data;
using System.Windows.Forms;
using UMS.Global;

namespace UMS.People
{
    public partial class frmListPeople : Form
    {
        private DataTable _dtPeople = clsPerson.GetAllPeople();

        public frmListPeople()
        {
            InitializeComponent();
        }

        private void _RefreshData()
        {
            _dtPeople = clsPerson.GetAllPeople();
            dgvPeople.DataSource = _dtPeople;
            lblRecords.Text = dgvPeople.Rows.Count.ToString();
        }

        private void frmPeople_Load(object sender, EventArgs e)
        {
            _RefreshData();
            bool canModify = clsGlobal.CanModify();
            btnAdd.Visible=canModify;
            btnDelete.Visible=canModify;
            btnEdit.Visible=canModify;
            addNewPersonToolStripMenuItem.Visible=canModify;
            editToolStripMenuItem.Visible=canModify;
            deleteToolStripMenuItem.Visible=canModify;
            cbFilterBy.SelectedIndex = 0;
            if (dgvPeople.Rows.Count > 0)
            {
                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 100;
                dgvPeople.Columns[1].HeaderText = "Full Name";
                dgvPeople.Columns[1].Width = 250;
                dgvPeople.Columns[2].HeaderText = "Phone";
                dgvPeople.Columns[2].Width = 100;
                dgvPeople.Columns[3].HeaderText = "Email";
                dgvPeople.Columns[3].Width = 250;
                dgvPeople.Columns[4].HeaderText = "Date Of Birth";
                dgvPeople.Columns[4].Width = 150;
                dgvPeople.Columns[5].HeaderText = "Gender";
                dgvPeople.Columns[5].Width = 70;
                dgvPeople.Columns[6].HeaderText = "Address";
                dgvPeople.Columns[6].Width = 250;
            }
        }

        private void benClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _RefreshData();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmAddUpdatePerson(PersonID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmAddUpdatePerson(PersonID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to delete Person ["
                + dgvPeople.CurrentRow.Cells[0].Value + "]",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsPerson.Delete((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Person Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "Person was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to delete Person ["
                + dgvPeople.CurrentRow.Cells[0].Value + "]",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsPerson.Delete((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show(
                        "Person Deleted Successfully.",
                        "Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _RefreshData();
                }
                else
                {
                    MessageBox.Show(
                        "Person was not deleted because it has data linked to it.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "PersonID":
                    FilterColumn = "PersonID";
                    break;
                case "Name":
                    FilterColumn = "Name";
                    break;
                case "Phone":
                    FilterColumn = "Phone";
                    break;
                case "Email":
                    FilterColumn = "Email";
                    break;
                case "Gender":
                    FilterColumn = "Gender";
                    break;
                case "Address":
                    FilterColumn = "Address";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblRecords.Text = dgvPeople.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "PersonID")
            {
                _dtPeople.DefaultView.RowFilter =
                    string.Format(
                        "[{0}] = {1}",
                        FilterColumn,
                        txtFilter.Text.Trim());
            }
            else
            {
                _dtPeople.DefaultView.RowFilter =
                    string.Format(
                        "[{0}] LIKE '{1}%'",
                        FilterColumn,
                        txtFilter.Text.Trim());
            }
            lblRecords.Text = dgvPeople.Rows.Count.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFilterBy.SelectedIndex != 0);
            if (cbFilterBy.SelectedIndex != 0)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }
    }
}