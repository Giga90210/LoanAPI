using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAccountantService
    {
        User BlockUser(int userId);
        User UnblockUser(int userId);
        Accountant Login(Accountant loginModel);
        Accountant Register(Accountant accountant);
        Accountant GetAccountantById(int id);

    }
}
