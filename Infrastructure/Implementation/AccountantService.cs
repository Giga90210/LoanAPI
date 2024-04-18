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
    public class AccountantService : IAccountantService
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _appDbContext;

        public AccountantService(AppDbContext appDbContext, IUserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
        }
        public User BlockUser(int userId)
        {
            var userToBlock = _userService.GetUserById(userId);
            if (userToBlock == null) {
                return null;
            }
            userToBlock.IsBlocked = true;
            _appDbContext.SaveChanges();
            return userToBlock;
        }
        public User UnblockUser(int userId)
        {
            var userToUnblock = _userService.GetUserById(userId);
            if (userToUnblock == null)
            {
                return null;
            }
            userToUnblock.IsBlocked = false;
            _appDbContext.SaveChanges();
            return userToUnblock;
        }

        public Accountant Login(Accountant loginModel)
        {
            //if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(password))
            //{
            //    return null;
            //}
            var accountant = _appDbContext.Accountants.FirstOrDefault(x => x.Email == loginModel.Email);
            if (accountant != null && BCrypt.Net.BCrypt.Verify(loginModel.Password, accountant.Password)) {
                return accountant;
            }
            return null;

        }

        public Accountant Register(Accountant registerModel)
        {
            //if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(password)) {
            //    return null; 
            //}

            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);
            registerModel.Password = hashedPassword;
            //accountant.Email = $"{accountant.FirstName}.{accountant.LastName}@accountant.gmail.com";
            _appDbContext.Accountants.Add(registerModel);
            _appDbContext.SaveChanges();
            //var newAccountant = new Accountant
            //{
            //    FirstName = firstName,
            //    LastName = lastName,
            //    Email = $"{firstName}.{lastName}@accountant.gmail.com",
            //    Password = hashedPassword
            //};
            return registerModel;
        }


        public Accountant GetAccountantById(int id) => _appDbContext.Accountants.FirstOrDefault(x => x.Id == id);
    }
}
