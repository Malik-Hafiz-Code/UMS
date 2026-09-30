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
    public class clsStudent
    {
        public enum enMode { Add, Update }
        public enMode Mode = enMode.Add;

        public int StudentID { get; set; }
        public int PersonID { get; set; }
        public clsPerson PersonInfo { get; set; }
        public clsDepartment Department { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public enum enStatus
        {
            Active,
            Graduated,
            Suspended,
            Dropped
        }
        public enStatus Status { get; set; }
        public decimal GPA { get; set; }

        public clsStudent()
        {
            StudentID = -1;
            PersonID = -1;
            PersonInfo = null;
            Department = null;
            EnrollmentDate = DateTime.Now;
            Status =enStatus.Active;
            GPA = 0;
            Mode = enMode.Add;
        }

        private clsStudent(int studentID, int personID, int departmentID, DateTime enrollmentDate,
            enStatus status, decimal gpa)
        {
            StudentID = studentID;
            PersonID = personID;
            PersonInfo = clsPerson.Find(personID);
            Department = clsDepartment.Find(departmentID);
            EnrollmentDate = enrollmentDate;
            Status = status;
            GPA = gpa;
            Mode = enMode.Update;
        }

        private bool _AddNewStudent()
        {
            StudentID = clsStudentData.AddNewStudent(PersonID, Department.DepartmentID
                , EnrollmentDate,Status.ToString(), GPA);
            return StudentID != -1;
        }

        private bool _UpdateStudent()
        {
            return clsStudentData.UpdateStudent(StudentID, PersonID
                , Department.DepartmentID, EnrollmentDate, Status.ToString(), GPA);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewStudent())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateStudent();
            }
            return false;
        }

        public static clsStudent Find(int studentID)
        {
            int personID = -1, departmentID = -1;
            DateTime enrollmentDate = DateTime.Now;
            string status = "";
            decimal gpa = 0;

            bool isFound = clsStudentData.GetStudentInfoByStudentID(studentID, ref personID, ref departmentID,
                ref enrollmentDate, ref status, ref gpa);

            if (isFound)
            {
                enStatus enStatus;
                if (Enum.TryParse<enStatus>(status, true, out enStatus))
                    return new clsStudent(studentID, personID, departmentID, enrollmentDate, enStatus, gpa);
            }
            return null;
        }

        public static bool Delete(int studentID) => clsStudentData.DeleteStudent(studentID);
        public bool Delete() => clsStudentData.DeleteStudent(StudentID);

        public static bool IsStudentExist(int studentID) => clsStudentData.IsStudentExist(studentID);
        public bool IsStudentExist() => clsStudentData.IsStudentExist(StudentID);

        public static bool IsPersonStudent(int personID) => clsStudentData.IsPersonStudent(personID);
        public bool IsPersonStudent() => clsStudentData.IsPersonStudent(PersonID);

        public static DataTable GetAllStudents() => clsStudentData.GetAllStudents();

        public static int CountStudents
        {
            get { return clsStudentData.CountStudents(); }
        }
        public static bool IsStudentByPersonID(int PersonID)
        {
            return clsStudentData.IsStudentByPersonID(PersonID);
        }
    }
}