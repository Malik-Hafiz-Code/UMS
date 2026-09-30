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
    public class clsDepartment
    {
        public enum enMode { Add, Update }
        public enMode Mode = enMode.Add;

        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string Building { get; set; }

        public clsDepartment()
        {
            DepartmentID = -1;
            Name = "";
            Building = "";
            Mode = enMode.Add;
        }

        private clsDepartment(int departmentID, string name, string building)
        {
            DepartmentID = departmentID;
            Name = name;
            Building = building;
            Mode = enMode.Update;
        }

        private bool _AddNewDepartment()
        {
            DepartmentID = clsDepartmentData.AddNewDepartment(Name, Building);
            return DepartmentID != -1;
        }

        private bool _UpdateDepartment()
        {
            return clsDepartmentData.UpdateDepartment(DepartmentID, Name, Building);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewDepartment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateDepartment();
            }
            return false;
        }

        public static clsDepartment Find(int departmentID)
        {
            string name = "", building = "";
            bool isFound = clsDepartmentData.GetDepartmentInfoByDepartmentID(departmentID, ref name, ref building);
            if (isFound)
                return new clsDepartment(departmentID, name, building);
            else
                return null;
        }

        public static bool Delete(int departmentID) => clsDepartmentData.DeleteDepartment(departmentID);
        public bool Delete() => clsDepartmentData.DeleteDepartment(DepartmentID);

        public static bool IsDepartmentExist(int departmentID) => clsDepartmentData.IsDepartmentExist(departmentID);
        public bool IsDepartmentExist() => clsDepartmentData.IsDepartmentExist(DepartmentID);

        public static DataTable GetAllDepartments() => clsDepartmentData.GetAllDepartments();

        public static int CountDepartments
        {
            get { return clsDepartmentData.CountDepartments(); }
        }
    }
}