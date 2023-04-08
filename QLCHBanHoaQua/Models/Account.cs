using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLCHBanHoaQua.Controllers;

namespace QLCHBanHoaQua.Models
{
    class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role VaiTro { get; set; }
    }
}
