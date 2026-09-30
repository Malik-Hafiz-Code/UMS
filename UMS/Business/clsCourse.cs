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
    public class clsCourse
    {
        public enum enMode { Add, Update }
        public enMode Mode = enMode.Add;

        public int CourseID { get; set; }
        public clsDepartment Department { get; set; }
        public string Name { get; set; }
        public int Credit { get; set; }
        public int AcademicLevel { get; set; }

        public clsCourse()
        {
            CourseID = -1;
            Department = null;
            Name = "";
            Credit = 0;
            AcademicLevel = 0;
            Mode = enMode.Add;
        }

        private clsCourse(int courseID, int departmentID, string name, int credit, int academicLevel)
        {
            CourseID = courseID;
            Department = clsDepartment.Find(departmentID);
            Name = name;
            Credit = credit;
            AcademicLevel = academicLevel;
            Mode = enMode.Update;
        }

        private bool _AddNewCourse()
        {
            CourseID = clsCourseData.AddNewCourse(Department.DepartmentID, Name, Credit, AcademicLevel);
            return CourseID != -1;
        }

        private bool _UpdateCourse()
        {
            return clsCourseData.UpdateCourse(CourseID, Department.DepartmentID, Name, Credit, AcademicLevel);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewCourse())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateCourse();
            }
            return false;
        }

        public static clsCourse Find(int courseID)
        {
            int departmentID = -1;
            string name = "";
            int credit = 0, academicLevel = 0;

            bool isFound = clsCourseData.GetCourseInfoByCourseID(courseID, ref departmentID, ref name,
                ref credit, ref academicLevel);

            if (isFound)
                return new clsCourse(courseID, departmentID, name, credit, academicLevel);
            else
                return null;
        }

        public static bool Delete(int courseID) => clsCourseData.DeleteCourse(courseID);
        public bool Delete() => clsCourseData.DeleteCourse(CourseID);

        public static bool IsCourseExist(int courseID) => clsCourseData.IsCourseExist(courseID);
        public bool IsCourseExist() => clsCourseData.IsCourseExist(CourseID);

        public static DataTable GetAllCourses() => clsCourseData.GetAllCourses();
    }
}