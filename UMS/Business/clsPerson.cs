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
    public class clsPerson
    {
        public enum enMode { Add,Update}
        public enMode Mode= enMode.Add;
        public int PersonID {  get; set; }
        public string Name { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender {  get; set; }
        public string Address {  get; set; }

        public clsPerson()
        {
            PersonID=-1;
            Name="";
            Phone="";
            Email="";
            DateOfBirth=DateTime.Now;
            Gender=' ';
            Address="";
            Mode=enMode.Add;
        }
        private clsPerson(int personID,string name,string phone,string email
            , DateTime dateOfBirth,char gender,string address)
        {
            PersonID=personID;
            Name=name;
            Phone=phone;
            Email=email;
            DateOfBirth=dateOfBirth;
            Gender=gender;
            Address=address;
            Mode=enMode.Update;
        }
        private bool _AddNewPerson()
        {
            PersonID=clsPersonData.AddNewPerson(Name, Phone, Email, DateOfBirth, Gender, Address);
            return PersonID!=-1;
        }
        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(PersonID, Name, Phone, Email, DateOfBirth, Gender, Address);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:
                    if (_AddNewPerson())
                    {
                        Mode=enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }
        public static clsPerson Find(int personID)
        {
            string name = "", phone = "", email = "", address = "";
            DateTime dateOfBirth = DateTime.Now;
            char gender = ' ';
            bool isFound=clsPersonData.GetPersonInfoByPersonID(personID,ref  name
                ,ref phone,ref email,ref dateOfBirth,ref gender,ref address);
            if (isFound)
                return new clsPerson(personID, name, phone, email, dateOfBirth, gender, address);
            else
                return null;
        }
        public static bool Delete(int personID) => clsPersonData.DeletePerson(personID);
        public bool Delete() => clsPersonData.DeletePerson(PersonID);
        public static bool IsPersonExist(int personID) => clsPersonData.IsPersonExist(personID);
        public bool IsPersonExist() => clsPersonData.IsPersonExist(PersonID);
        public static DataTable GetAllPeople() => clsPersonData.GetAllPeople();

        public static int CountPeople
        {
            get { return clsPersonData.CountPeople(); }
        }
    }
}
