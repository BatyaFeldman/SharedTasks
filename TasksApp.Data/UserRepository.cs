using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TasksApp.Data
{
    public class UserRepository
    {
        private string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void signUp(string FirstName, string LastName, string EmailAddress, string Password)
        {
            string passSalt = PasswordHelper.GenerateSalt();
            string passHash = PasswordHelper.HashPassword(Password, passSalt);

            User user = new User();
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.EmailAddress = EmailAddress;
            user.PassSalt = passSalt;
            user.PassHash = passHash;
            
            using (var context=new UsersTasksDataContext(_connectionString))
            {
                context.Users.InsertOnSubmit(user);
                context.SubmitChanges();
            }

        }

        public User signedInUser(string EmailAddress, string Password)
        {
             using(var context=new UsersTasksDataContext(_connectionString))
            {
                User user = context.Users.Where(u => u.EmailAddress == EmailAddress).FirstOrDefault();

                if( PasswordHelper.PasswordMatch(Password, user.PassSalt, user.PassHash))
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public int getIdByEmail(string email)
        {
            using(var context = new UsersTasksDataContext(_connectionString))
            {
                User user = context.Users.Where(u => u.EmailAddress == email).FirstOrDefault();
                return user.Id;
                
            }
        }

        public User GetById(int id)
        {
            using(var context=new UsersTasksDataContext(_connectionString))
            {
                return context.Users.Where(u=>u.Id==id).FirstOrDefault();
            }
        }
    }

    public static class PasswordHelper
    {
        public static string GenerateSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[10];
            provider.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static string HashPassword(string password, string salt)
        {
            SHA256Managed crypt = new SHA256Managed();
            string combinedString = password + salt;
            byte[] combined = Encoding.Unicode.GetBytes(combinedString);

            byte[] hash = crypt.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        public static bool PasswordMatch(string userInput, string salt, string passwordHash)
        {
            string userInputHash = HashPassword(userInput, salt);
            return passwordHash == userInputHash;
        }
    }
}
