using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Name { get; set; } = "Default Name";
        public string SName { get; set; } = "Default SurName";

        public string Password { get; set; }
    }
}
