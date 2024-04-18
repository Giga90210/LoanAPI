using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Implementation;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class LoanServiceTests
    {
        private readonly Mock<ILoanService> _loanService;
        private readonly Mock<AppDbContext> _appDbContext;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<DbSet<Loan>> _mockLoanSet;

        public LoanServiceTests()
        {
            _loanService = new Mock<ILoanService>();
            _appDbContext = new Mock<AppDbContext>();
            _mockUserService = new Mock<IUserService>();
            _mockLoanSet = new Mock<DbSet<Loan>>();
            _appDbContext.Setup(db => db.Loans).Returns(_mockLoanSet.Object);

            
        }

        
        [Fact]
        public void AddLoan_UserIsBlocked_ShouldReturnNull()
        {
            var loan = new Loan { UserId = 1 };
            var user = new User { IsBlocked = true };

            _mockUserService.Setup(x => x.GetUserById(1)).Returns(user);

            var result = _loanService.Object.AddLoan(loan);

            Assert.Null(result);
        }

    }
}
