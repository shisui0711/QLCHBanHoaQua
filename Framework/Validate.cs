using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework
{
    public class Validate
    {
        public static bool Phone(string phone)
        {
            var regex1 = new Regex(@"^0[1-9]\d{8}$");
            var regex2 = new Regex(@"^+?84[1-9]\d{8}$");
            if (regex1.IsMatch(phone) || regex2.IsMatch(phone)) return true;
            return false;
        }
        public static bool Gender(string gender)
        {
            if (gender == "nam" || gender == "Nam" || gender == "Nu" || gender == "nu" || gender == "Nữ" || gender == "nữ") return true;
            return false;
        }
    }
}
