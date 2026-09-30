using DataAccess;
using System;
using System.Data;

namespace Business
{
    public class clsUser
    {
        public enum enMode
        {
            Add,
            Update
        }

        public enum enRole
        {
            Admin,
            Doctor,
            Student
        }

        public enMode Mode { get; set; }

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public enRole Role { get; set; }
        public bool IsActive { get; set; }

        public clsPerson PersonInfo { get; set; }


        public clsUser()
        {
            UserID = -1;
            PersonID = -1;
            Username = "";
            Password = "";
            Role = enRole.Admin;
            IsActive = true;
            Mode = enMode.Add;
        }


        private clsUser(
            int userID,
            int personID,
            string userName,
            string password,
            enRole role,
            bool isActive)
        {
            UserID = userID;
            PersonID = personID;
            Username = userName;
            Password = password;
            Role = role;
            IsActive = isActive;

            PersonInfo = clsPerson.Find(PersonID);

            Mode = enMode.Update;
        }


        private bool _AddNewUser()
        {
            UserID = clsUserData.AddNewUser(
                PersonID,
                Username,
                Password,
                Role.ToString(),
                IsActive);

            return UserID != -1;
        }


        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(
                UserID,
                PersonID,
                Username,
                Password,
                Role.ToString(),
                IsActive);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.Add:

                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }

                    return false;


                case enMode.Update:

                    return _UpdateUser();
            }

            return false;
        }


        public static bool Delete(int userID)
        {
            return clsUserData.DeleteUser(userID);
        }


        public static clsUser FindByUsernameAndPassword(
            string username,
            string password)
        {
            int userID = -1;
            int personID = -1;

            string role = "";
            bool isActive = true;

            bool isFound = clsUserData.FindByUsernameAndPassword(
                username,
                password,
                ref userID,
                ref personID,
                ref role,
                ref isActive);

            if (isFound)
            {
                enRole userRole;

                if (Enum.TryParse<enRole>(
                    role,
                    true,
                    out userRole))
                {
                    return new clsUser(
                        userID,
                        personID,
                        username,
                        password,
                        userRole,
                        isActive);
                }
            }

            return null;
        }


        public static clsUser FindByUserID(int userID)
        {
            int personID = -1;

            string username = "";
            string password = "";
            string role = "";

            bool isActive = true;

            bool isFound = clsUserData.GetUserInfoByUserID(
                userID,
                ref personID,
                ref username,
                ref password,
                ref role,
                ref isActive);

            if (isFound)
            {
                enRole userRole;

                if (Enum.TryParse<enRole>(
                    role,
                    true,
                    out userRole))
                {
                    return new clsUser(
                        userID,
                        personID,
                        username,
                        password,
                        userRole,
                        isActive);
                }
            }

            return null;
        }


        public static clsUser FindByPersonID(int personID)
        {
            int userID = -1;

            string username = "";
            string password = "";
            string role = "";

            bool isActive = true;

            bool isFound = clsUserData.GetUserInfoByPersonID(
                ref userID,
                personID,
                ref username,
                ref password,
                ref role,
                ref isActive);

            if (isFound)
            {
                enRole userRole;

                if (Enum.TryParse<enRole>(
                    role,
                    true,
                    out userRole))
                {
                    return new clsUser(
                        userID,
                        personID,
                        username,
                        password,
                        userRole,
                        isActive);
                }
            }

            return null;
        }


        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }


        public static bool IsUserExist(int userID)
        {
            return clsUserData.IsUserExist(userID);
        }


        public static bool IsUserExistByUsername(string username)
        {
            return clsUserData.IsUserExist(username);
        }


        public static bool IsUserExistByPersonID(int personID)
        {
            return clsUserData.IsUserExistByPersonID(personID);
        }


        public bool ChangePassword(string newPassword)
        {
            return clsUserData.ChangePassword(
                UserID,
                newPassword);
        }


        public static bool ChangePassword(
            int userID,
            string newPassword)
        {
            return clsUserData.ChangePassword(
                userID,
                newPassword);
        }


        public static int CountAllUsers
        {
            get
            {
                return clsUserData.CountAllUsers();
            }
        }


        public static int CountActiveUsers
        {
            get
            {
                return clsUserData.CountActiveUsers();
            }
        }
    }
}