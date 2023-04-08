﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class Format
    {
        public static string Name(string name)
        {
            string tmp = string.Join(" ", (name.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries)));
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tmp.ToLower());
        }
        public static string SplitLastName(string fullname)
        {
            int pos = fullname.Trim().LastIndexOf(' ');
            return fullname.Substring(pos + 1);
        }
    }
}
