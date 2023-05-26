using System.Text.RegularExpressions;

namespace Framework
{
    public class Validate
    {
        public static bool Phone(string phone)
        {
            if (Regex.IsMatch(phone, @"^0[1-9]\d{8}$") || Regex.IsMatch(phone, @"^+?84[1-9]\d{8}$")) return true;
            return false;
        }
        public static bool Gender(string gender)
        {
            if (gender == "nam" || gender == "Nam" || gender == "Nu" || gender == "nu" || gender == "Nữ" || gender == "nữ") return true;
            return false;
        }
    }
}
