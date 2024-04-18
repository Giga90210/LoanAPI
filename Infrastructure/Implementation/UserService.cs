using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Loan> GetMyLoans(int id)
        {
            var loans = _appDbContext.Loans.Where(x=>x.UserId == id).ToList();
            if(loans == null || loans.Count == 0)
            {
                return null;
            }
            return loans;
        }

        public User GetUserById(int id) => _appDbContext.Users.FirstOrDefault(u => u.Id == id);

        public IEnumerable<User> GetUsers() => _appDbContext.Users;

        public User Login(User loginModel)
        {
            var user = _appDbContext.Users.FirstOrDefault(x => x.Email == loginModel.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password)) 
            {
                return null;   
            }
            return user;

        }

        public User Register(User user)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            _appDbContext.Add(user);
            _appDbContext.SaveChanges();
            return user;
        }
    }
}
