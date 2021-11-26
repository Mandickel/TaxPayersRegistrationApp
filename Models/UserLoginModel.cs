using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaxPayersRegistration.Models
{
    public class UserLoginModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string  Password { get; set; }
    }
}