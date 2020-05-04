using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackOverflowClone.Data
{
    public class AccountRepository
    {
        private readonly string _connectionString;
        public AccountRepository(string conn)
        {
            _connectionString = conn;
        }
        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;
            using(var context = new QuestionContext(_connectionString))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        public User LogIn(string email, string password)
        {
            var user = GetUserByEmail(email);
            if(user == null)
            {
                return null;
            }
            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (passwordValid)
            {
                return user;
            }
            return null;
        }
        public User GetUserByEmail(string email)
        {
            using(var context = new QuestionContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }
    }
}
