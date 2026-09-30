using Business;
using System;
using System.Windows.Forms;

namespace UMS.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public DataBackEventHandler DataBack;
        public enum enMode{ Add, Update}

        private enMode _Mode = enMode.Add;
        private int _PersonID = -1;
        private clsPerson _Person;

        public frmAddUpdatePerson()
        {
            InitializeComponent();

            _Mode = enMode.Add;
            _Person = new clsPerson();

            lblTitle.Text = "Add New Person";
            this.Text=lblTitle.Text;
            dtpDateOfBirth.MinDate = new DateTime(1970, 1, 1);
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
        }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
            _Mode = enMode.Update;

            lblTitle.Text = "Update Person";
            this.Text= lblTitle.Text;
            dtpDateOfBirth.MinDate = new DateTime(1970, 1, 1);
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
        }

        private void _Reset()
        {
            lblPersonID.Text = "???";

            txtName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();

            rbMale.Checked = true;

            dtpDateOfBirth.Value = DateTime.Now.AddYears(-18);
        }

        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show(
                    "Person Not Found",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                this.Close();
                return;
            }

            lblPersonID.Text = _Person.PersonID.ToString();

            txtName.Text = _Person.Name;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;

            rbMale.Checked = (_Person.Gender == 'M');
            rbFemale.Checked = (_Person.Gender == 'F');

            dtpDateOfBirth.Value = _Person.DateOfBirth;
        }

        private bool _ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(
                    "Please enter the person's name.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show(
                    "Please enter the phone number.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txtPhone.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show(
                    "Please enter the email.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show(
                    "Please enter the address.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txtAddress.Focus();
                return false;
            }

            return true;
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateInput())
                return;

            _Person.Name = txtName.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Gender = rbMale.Checked ? 'M' : 'F';

            if (_Person.Save())
            {
                if (_Mode == enMode.Add)
                {
                    _PersonID = _Person.PersonID;

                    lblPersonID.Text = _PersonID.ToString();

                    MessageBox.Show(
                        $"Person Added [{_PersonID}] Successfully",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    _Mode = enMode.Update;
                    DataBack?.Invoke(this, _Person.PersonID);
                    lblTitle.Text = "Update Person";
                }
                else
                {
                    MessageBox.Show(
                        "Person Updated Successfully",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(
                    "Operation Failed",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}