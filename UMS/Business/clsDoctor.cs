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
    public class clsDoctor
    {
        public enum enMode { Add, Update }
        public enMode Mode = enMode.Add;

        public int DoctorID { get; set; }
        public int PersonID { get; set; }
        public clsPerson PersonInfo { get; set; }
        public clsDepartment Department { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }

        public clsDoctor()
        {
            DoctorID = -1;
            PersonID = -1;
            PersonInfo = null;
            Department = null;
            Salary = 0;
            IsActive = true;
            Mode = enMode.Add;
        }

        private clsDoctor(int doctorID, int personID, int departmentID, decimal salary, bool isActive)
        {
            DoctorID = doctorID;
            PersonID = personID;
            PersonInfo = clsPerson.Find(personID);
            Department = clsDepartment.Find(departmentID);
            Salary = salary;
            IsActive = isActive;
            Mode = enMode.Update;
        }

        private bool _AddNewDoctor()
        {
            DoctorID = clsDoctorData.AddNewDoctor(PersonID, Department.DepartmentID, Salary, IsActive);
            return DoctorID != -1;
        }

        private bool _UpdateDoctor()
        {
            return clsDoctorData.UpdateDoctor(DoctorID, PersonID, Department.DepartmentID, Salary, IsActive);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewDoctor())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateDoctor();
            }
            return false;
        }

        public static clsDoctor Find(int doctorID)
        {
            int personID = -1, departmentID = -1;
            decimal salary = 0;
            bool isActive = true;

            bool isFound = clsDoctorData.GetDoctorInfoByDoctorID(doctorID, ref personID, ref departmentID,
                ref salary, ref isActive);

            if (isFound)
                return new clsDoctor(doctorID, personID, departmentID, salary, isActive);
            else
                return null;
        }

        public static bool Delete(int doctorID) => clsDoctorData.DeleteDoctor(doctorID);
        public bool Delete() => clsDoctorData.DeleteDoctor(DoctorID);

        public static bool IsDoctorExist(int doctorID) => clsDoctorData.IsDoctorExist(doctorID);
        public bool IsDoctorExist() => clsDoctorData.IsDoctorExist(DoctorID);

        public static bool IsPersonDoctor(int personID) => clsDoctorData.IsPersonDoctor(personID);
        public bool IsPersonDoctor() => clsDoctorData.IsPersonDoctor(PersonID);

        public static DataTable GetAllDoctors() => clsDoctorData.GetAllDoctors();

        public static int CountDoctors
        {
            get { return clsDoctorData.CountDoctors(); }
        }
        public static bool IsDoctorByPersonID(int PersonID)
        {
            return clsDoctorData.IsDoctorByPersonID(PersonID);
        }
    }
}