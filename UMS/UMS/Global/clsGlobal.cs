using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Global
{
    internal static class clsGlobal
    {
       public static clsUser CurrentUser;
        public static string Username
        {
            get { return CurrentUser?.Username; }
        }
        public static bool CanModify()
        {
            return CurrentUser.Role ==clsUser.enRole.Admin;
        }
    }
}
