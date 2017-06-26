using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPITs.Models
{
    public class Account
    {
        public LoginParameters AccountParameters { get; set; }
        public decimal Money { get; set; }

    }
}
