using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation
{
    public class LoanService : ILoanService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;
        public LoanService(AppDbContext appDbContext, IUserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
        }
        public Loan AddLoan(Loan loan)
        {
            var user = _userService.GetUserById(loan.UserId);
            if (user.IsBlocked)
            {
                return null;
            }
            user.Loans.Add(loan);
            _appDbContext.Loans.Add(loan);
            _appDbContext.SaveChanges();
            return loan;
        }

        public Loan GetLoan(int id) => _appDbContext.Loans.FirstOrDefault(x => x.Id == id);

        public Loan UpdateLoan(Loan loan, int id)
        {
            var loanToUpdate = GetLoan(id);
            if(loanToUpdate == null)
            {
                return null;
            }
            loanToUpdate.LoanAmount = loan.LoanAmount;
            loanToUpdate.LoanType = loan.LoanType;
            loanToUpdate.Currency = loan.Currency;
            loanToUpdate.LoanPeriod = loan.LoanPeriod;
            loanToUpdate.Status = loan.Status;
            _appDbContext.SaveChanges();
            return loanToUpdate;

        }


        public Loan DeleteLoan(int id)
        {
            var loanToDelete = GetLoan(id);
            _appDbContext.Remove(loanToDelete);
            _appDbContext.SaveChanges();
            return loanToDelete;
        }

        public IEnumerable<Loan> GetLoansByUserId(int userId) => _appDbContext.Loans.Where(x=>x.UserId == userId).ToList();
    }
}
