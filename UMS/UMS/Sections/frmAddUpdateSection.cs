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

namespace UMS.Sections
{
    public partial class frmAddUpdateSection : Form
    {
        private int _SectionID;
        private clsSection _Section;
        private enum enMode { Add,Update}
        private enMode _Mode= enMode.Add;
        public frmAddUpdateSection()
        {
            InitializeComponent();
            _Mode = enMode.Add;
            _Section=new clsSection();
            lblTitle.Text="Add New Section";
            this.Text=lblTitle.Text;
        }
        public frmAddUpdateSection(int SectionID)
        {
            InitializeComponent();
            _SectionID = SectionID;
            _Mode=enMode.Update;
            lblTitle.Text="Update Section";
            this.Text=lblTitle.Text;
        }
        private void _LoadCourseName()
        {
            DataTable dt=clsCourse.GetAllCourses();
            cbCourseName.DataSource=dt;
            cbCourseName.DisplayMember="Name";
            cbCourseName.ValueMember="CourseID";
        }
        private void _LoadDocterName()
        {
            DataTable dt = clsDoctor.GetAllDoctors();
            cbDoctorName.DataSource=dt;
            cbDoctorName.DisplayMember="Name";
            cbDoctorName.ValueMember="DoctorID";
        }
        private void _LoadData()
        {
            _Section=clsSection.Find(_SectionID);
            if (_Section==null)
            {
                MessageBox.Show($"Not Found {_SectionID}","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblSectionID.Text=_SectionID.ToString();
            cbCourseName.SelectedValue=_Section.Course.CourseID;
            cbDoctorName.SelectedValue=_Section.Doctor.DoctorID;
            txtYear.Text=_Section.Year.ToString();
            txtRoom.Text=_Section.Room.ToString();
            cbSemester.Text=_Section.Semester.ToString();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmAddUpdateSection_Load(object sender, EventArgs e)
        {
            _LoadCourseName();
            _LoadDocterName();
            if (_Mode==enMode.Update)
                _LoadData();
            else
                cbSemester.SelectedIndex=0;
        }
      
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtYear.Text.Trim()=="")
            {
                MessageBox.Show("Fill Year","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtRoom.Text.Trim()=="")
            {
                MessageBox.Show("Fill Room", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int selectedCourseID = (int)cbCourseName.SelectedValue;
            int selectedDoctorID=(int)cbDoctorName.SelectedValue;
            _Section.Course=clsCourse.Find(selectedCourseID);
            _Section.Doctor=clsDoctor.Find(selectedDoctorID);
            int.TryParse(txtYear.Text,out int year);
            _Section.Year=year;
            int.TryParse(txtRoom.Text,out int room);
            _Section.Room=room;
            _Section.Semester=(clsSection.enSemester)Enum.Parse
                (typeof(clsSection.enSemester), cbSemester.Text);
            if (_Mode == enMode.Add)
            {
                if (_Section.Save())
                {
                    _SectionID = _Section.SectionID;
                    lblSectionID.Text = _SectionID.ToString();
                    MessageBox.Show(
                        $"Section [{_SectionID}] added successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    _Mode = enMode.Update;
                    lblTitle.Text = "Update Section";
                    this.Text = lblTitle.Text;
                }
                else
                {
                    MessageBox.Show(
                        "Section was not added.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                if (_Section.Save())
                {
                    MessageBox.Show(
                        "Section updated successfully.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Section was not updated.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}
