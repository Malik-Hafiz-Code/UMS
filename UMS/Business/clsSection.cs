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
    public class clsSection
    {
        public enum enMode { Add, Update }
        public enMode Mode = enMode.Add;

        public int SectionID { get; set; }
        public clsCourse Course { get; set; }
        public clsDoctor Doctor { get; set; }
        public int Year { get; set; }
        public enum enSemester { Fall,Spring,Summer}
        public enSemester Semester {  get; set; }
        public int Room { get; set; }

        public clsSection()
        {
            SectionID = -1;
            Course = null;
            Doctor = null;
            Year = DateTime.Now.Year;
            Semester =enSemester.Fall;
            Room = 0;
            Mode = enMode.Add;
        }

        private clsSection(int sectionID, int courseID, int doctorID, int year, enSemester semester, int room)
        {
            SectionID = sectionID;
            Course = clsCourse.Find(courseID);
            Doctor = clsDoctor.Find(doctorID);
            Year = year;
            Semester = semester;
            Room = room;
            Mode = enMode.Update;
        }

        private bool _AddNewSection()
        {
            SectionID = clsSectionData.AddNewSection(Course.CourseID,
                Doctor.DoctorID, Year, Semester.ToString(), Room);
            return SectionID != -1;
        }

        private bool _UpdateSection()
        {
            return clsSectionData.UpdateSection(SectionID, Course.CourseID
                , Doctor.DoctorID, Year, Semester.ToString(), Room);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewSection())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateSection();
            }
            return false;
        }

        public static clsSection Find(int sectionID)
        {
            int courseID = -1, doctorID = -1, year = 0, room = 0;
            string semester = "";

            bool isFound = clsSectionData.GetSectionInfoBySectionID(sectionID, ref courseID, ref doctorID,
                ref year, ref semester, ref room);

            if (isFound)
            {
                enSemester enSemester;
                if (Enum.TryParse<enSemester>(semester, true, out enSemester))
                    return new clsSection(sectionID, courseID, doctorID, year, enSemester, room);
            }
            return null;
        }

        public static bool Delete(int sectionID) => clsSectionData.DeleteSection(sectionID);
        public bool Delete() => clsSectionData.DeleteSection(SectionID);

        public static bool IsSectionExist(int sectionID) => clsSectionData.IsSectionExist(sectionID);
        public bool IsSectionExist() => clsSectionData.IsSectionExist(SectionID);

        public static DataTable GetAllSections() => clsSectionData.GetAllSections();
        public static DataTable GetAllSectionsRaw() => clsSectionData.GetAllSectionsRaw();
    }
}