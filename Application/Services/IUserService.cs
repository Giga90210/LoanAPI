using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserService
    {
        User Register(User user);
        User Login(User loginModel);
        IEnumerable<User> GetUsers();
        User GetUserById(int id);
        IEnumerable<Loan> GetMyLoans(int id);

    }
}
