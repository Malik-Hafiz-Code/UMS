using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Business
{
    public class clsEnrollment
    {
        public enum enMode { Add, Update }
        public enMode Mode = enMode.Add;

        public int EnrollmentID { get; set; }
        public clsStudent Student { get; set; }
        public clsSection Section { get; set; }
        public double Grade { get; set; }
        public enum enStatus
        {
            Active, Completed, Failed, Withdrawn
        }
        public enStatus Status { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public clsEnrollment()
        {
            EnrollmentID = -1;
            Student = null;
            Section = null;
            Grade = 0;
            Status =enStatus.Completed;
            EnrollmentDate = DateTime.Now;
            Mode = enMode.Add;
        }

        private clsEnrollment(int enrollmentID, int studentID, int sectionID, double grade,
            enStatus status, DateTime enrollmentDate)
        {
            EnrollmentID = enrollmentID;
            Student = clsStudent.Find(studentID);
            Section = clsSection.Find(sectionID);
            Grade = grade;
            Status = status;
            EnrollmentDate = enrollmentDate;
            Mode = enMode.Update;
        }

        private bool _AddNewEnrollment()
        {
            EnrollmentID = clsEnrollmentData.AddNewEnrollment(Student.StudentID, Section.SectionID, Grade,
                Status.ToString(), EnrollmentDate);
            return EnrollmentID != -1;
        }

        private bool _UpdateEnrollment()
        {
            return clsEnrollmentData.UpdateEnrollment(EnrollmentID, Student.StudentID, Section.SectionID,
                Grade, Status.ToString(), EnrollmentDate);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewEnrollment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateEnrollment();
            }
            return false;
        }

        public static clsEnrollment Find(int enrollmentID)
        {
            int studentID = -1, sectionID = -1;
            double grade = 0;
            string status = "";
            DateTime enrollmentDate = DateTime.Now;

            bool isFound = clsEnrollmentData.GetEnrollmentInfoByEnrollmentID(enrollmentID, ref studentID,
                ref sectionID, ref grade, ref status, ref enrollmentDate);

            if (isFound)
            {
                enStatus enStatus;
                if(Enum.TryParse<enStatus>(status,true,out enStatus))
                return new clsEnrollment(enrollmentID, studentID, sectionID, grade, enStatus, enrollmentDate);
            }
            return null;
        }

        public static bool Delete(int enrollmentID) => clsEnrollmentData.DeleteEnrollment(enrollmentID);
        public bool Delete() => clsEnrollmentData.DeleteEnrollment(EnrollmentID);

        public static bool IsEnrollmentExist(int enrollmentID) => clsEnrollmentData.IsEnrollmentExist(enrollmentID);
        public bool IsEnrollmentExist() => clsEnrollmentData.IsEnrollmentExist(EnrollmentID);

        public static bool IsStudentEnrolledInSection(int studentID, int sectionID) =>
            clsEnrollmentData.IsStudentEnrolledInSection(studentID, sectionID);

        public static DataTable GetAllEnrollments() => clsEnrollmentData.GetAllEnrollments();
    }
}