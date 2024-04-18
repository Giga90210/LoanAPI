using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Implementation;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace AccountantServiceTests
{
    public class AccountantServiceTests
    {
        private readonly Mock<IAccountantService> _accountantService;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<AppDbContext> _appDbContext;
        private readonly Mock<DbSet<User>> _mockUserSet;
        private readonly Mock<DbSet<Accountant>> _mockAccountantSet;
        public AccountantServiceTests()
        {
            _accountantService = new Mock<IAccountantService>();
            _userServiceMock = new Mock<IUserService>();
            _appDbContext = new Mock<AppDbContext>();
            _mockUserSet = new Mock<DbSet<User>>();
            _mockAccountantSet = new Mock<DbSet<Accountant>>();
            _appDbContext.Setup(m => m.Users).Returns(_mockUserSet.Object);
            _appDbContext.Setup(m => m.Accountants).Returns(_mockAccountantSet.Object);
        }

        public void SetupMockAccountantSet(List<Accountant> accountants)
        {
            var queryable = accountants.AsQueryable();
            var mockSet = new Mock<DbSet<Accountant>>();
            _mockAccountantSet.As<IQueryable<Accountant>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _mockAccountantSet.As<IQueryable<Accountant>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _mockAccountantSet.As<IQueryable<Accountant>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _mockAccountantSet.As<IQueryable<Accountant>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            _appDbContext.Setup(db => db.Accountants).Returns(mockSet.Object);
        }



        [Fact]
        public void BlockUser_Should_Return_Null_when_Id_Not_Found()
        {
            var userId = 1;
            _userServiceMock.Setup(x => x.GetUserById(userId)).Returns((User)null);
            
            var blockedUser = _accountantService.Object.BlockUser(userId);

            Assert.Null(blockedUser);

        }
        [Fact]
        public void BlockUser_Should_Block_User_When_Id_Is_Valid()
        {
            var userId = 1;
            _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(new User { Id = 1, IsBlocked = false });

            var blockedUser = _accountantService.Object.BlockUser(userId);

            Assert.Null(blockedUser);
        }



        [Fact]
        public void UnBlockUser_Should_Return_Null_when_Id_Not_Found()
        {
            var userId = 1;
            _userServiceMock.Setup(x => x.GetUserById(userId)).Returns((User)null);

            var blockedUser = _accountantService.Object.UnblockUser(userId);

            Assert.Null(blockedUser);

        }
        [Fact]
        public void UnBlockUser_Should_UnBlock_User_When_Id_Is_Valid()
        {
            var userId = 1;
            _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(new User { Id = 1, IsBlocked = false });

            var blockedUser = _accountantService.Object.UnblockUser(userId);

            Assert.Null(blockedUser);
        }

        [Fact]
        public void Login_Should_Return_Null_When_Input_Is_Invalid()
        {
            var accountants = new List<Accountant>
            {
                new Accountant { Email = "example@Email.com", Password = "Correct" }
            };
            SetupMockAccountantSet(accountants);

            var accountant = new Accountant
            {
                Email = "Wrong@Email.com",
                Password = "Wrong"
            };

            var loggedInAccountant = _accountantService.Object.Login(accountant);

            Assert.Null(loggedInAccountant);
        }

    }


}
