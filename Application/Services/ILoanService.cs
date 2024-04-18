using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ILoanService
    {
        Loan AddLoan(Loan loan);
        Loan GetLoan(int id);
        Loan UpdateLoan(Loan loan, int id);
        Loan DeleteLoan(int id);
        IEnumerable<Loan> GetLoansByUserId(int userId);

        Loan ApporveLoan(int id);
        Loan RejectLoan(int id);

    }
}
