using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public bool IsBlocked { get; set; } = false;
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = Role.User;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    }
}
