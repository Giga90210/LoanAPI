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

namespace AccountantServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<AppDbContext> _appDbContext;
        private readonly Mock<DbSet<User>> _mockUserSet;
        public UserServiceTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _appDbContext = new Mock<AppDbContext>();
            _mockUserSet = new Mock<DbSet<User>>();
            _appDbContext.Setup(m => m.Users).Returns(_mockUserSet.Object);
        }

        public void SetupMockUserSet(List<User> users)
        {
            var queryable = users.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            _appDbContext.Setup(db => db.Users).Returns(mockSet.Object);
        }

        [Fact]
        public void GetMyLoans_Should_Return_Empty_When_User_Not_Found()
        {
            var userId = 1;
            var users = new List<User> { new User { Id = 2 } };

            _userServiceMock.Setup(x => x.GetMyLoans(userId)).Returns(new List<Loan>());

            var userLoans = _userServiceMock.Object.GetMyLoans(userId);

            Assert.NotNull(userLoans);
            Assert.Empty(userLoans);
        }


        [Fact]
        public void GetMyLoans_Should_Return_Loan_List_When_User_Found()
        {
            var userId = 1;
            var loans = new List<Loan> { new Loan { Id = 111 } };
            var users = new List<User>
            {
                new User { Id = userId, Loans = loans }
            };

            _userServiceMock.Setup(x => x.GetUserById(userId)).Returns(users.FirstOrDefault(u => u.Id == userId));

            _userServiceMock.Setup(x => x.GetMyLoans(userId)).Returns(users.FirstOrDefault(u => u.Id == userId)?.Loans);

            var userLoans = _userServiceMock.Object.GetMyLoans(userId);

            Assert.NotEmpty(userLoans);
        }


        [Fact]
        public void GetUserById_Should_Return_Null_When_User_Does_Not_Exist()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 2, FirstName = "Jane Doe" }
            };
            SetupMockUserSet(users);


            // Act
            var result = _userServiceMock.Object.GetUserById(1);

            // Assert
            Assert.Null(result);
        }




    }
}
