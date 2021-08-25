using Jwt.Context;
using Jwt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            user.Id = _context.SaveChanges();
            return user;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(s=>s.Email==email);
        }

        public User GetById(int Id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == Id);
        }

        //public User (int Id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
