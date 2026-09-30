using Business;
using System;
using System.Windows.Forms;

namespace UMS.People.Controls
{
    public partial class ctrlPersonInfoWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;

            if (handler != null)
                handler(PersonID);
        }

        public ctrlPersonInfoWithFilter()
        {
            InitializeComponent();
        }

        private bool _ShowAddPerson = true;

        public bool ShowAddPerson
        {
            get { return _ShowAddPerson; }
            set
            {
                _ShowAddPerson = value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }

        public int PersonID
        {
            get { return ctrlPersonInfo1.PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonInfo1.PersonInfo; }
        }

        private void _FindNow()
        {
            if (!int.TryParse(txtFindByPersonID.Text.Trim(), out int personID))
                return;

            ctrlPersonInfo1.LoadPersonInfo(personID);

            if (ctrlPersonInfo1.PersonInfo!=null && FilterEnabled)
                PersonSelected(ctrlPersonInfo1.PersonID);
        }

        public void LoadInfo(int PersonID)
        {
            txtFindByPersonID.Text = PersonID.ToString();
            _FindNow();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtFindByPersonID.Text.Trim() == "")
            {
                Reset();
                MessageBox.Show(
                    "Please enter a Person ID to find.",
                    "Find Person",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            _FindNow();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += DataBackEvent;
            frm.ShowDialog();
        }

        public void DataBackEvent(object sender, int personID)
        {
            txtFindByPersonID.Text = personID.ToString();
            ctrlPersonInfo1.LoadPersonInfo(personID);
        }
        public void Reset()
        {
            ctrlPersonInfo1.Reset();
        }
        public void FilterFocus()
        {
            txtFindByPersonID.Focus();
        }
    }
}