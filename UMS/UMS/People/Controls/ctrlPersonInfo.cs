using Business;
using System;
using System.Windows.Forms;

namespace UMS.People.Controls
{
    public partial class ctrlPersonInfo : UserControl
    {
        private clsPerson _Person;
        private int _PersonID = -1;

        public ctrlPersonInfo()
        {
            InitializeComponent();
            Reset();
        }

        public int PersonID
        {
            get { return _PersonID; }
        }

        public clsPerson PersonInfo
        {
            get { return _Person; }
        }

        public void Reset()
        {
            _PersonID = -1;
            _Person = null;

            lblPersonID.Text = "???";
            lblName.Text = "???";
            lblPhone.Text = "???";
            lblEmail.Text = "???";
            lblDateOfBirth.Text = "???";
            lblGender.Text = "???";
            lblAddress.Text = "???";
        }

        private void _FillPersonInfo()
        {
            lblPersonID.Text = _Person.PersonID.ToString();
            lblName.Text = _Person.Name;
            lblPhone.Text = _Person.Phone;
            lblEmail.Text = _Person.Email;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblGender.Text = (_Person.Gender == 'M') ? "Male" : "Female";
            lblAddress.Text = _Person.Address;
        }

        public void LoadPersonInfo(int personID)
        {
            Reset();

            _Person = clsPerson.Find(personID);

            if (_Person == null)
            {
                MessageBox.Show(
                    "Person ID [" + personID + "] was not found.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            _PersonID = _Person.PersonID;
            _FillPersonInfo();
        }
    }
}