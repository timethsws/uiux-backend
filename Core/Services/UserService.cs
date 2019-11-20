using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Database;
using Core.Entities;
using Core.Model;

namespace Core.Services
{
    public class UserService
    {
        private readonly AppDbContext dbContext;

        private readonly SHA256 MySHA256;

        public UserService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            MySHA256 = SHA256.Create();
        }

        /// <summary>
        /// Add a new user to the system
        /// </summary>
        /// <param name="userModel"> user data</param>
        /// <returns>Added user</returns>
        public async Task<OperationOutput<ApplicationUser>> AddUser (ApplicationUserModel userModel)
        {
            if(userModel == null || userModel.Id != Guid.Empty)
            {
                return OperationOutput<ApplicationUser>.Failed("Arguments Null");
            }

            if(String.IsNullOrWhiteSpace(userModel.Email) || String.IsNullOrWhiteSpace(userModel.FirstName) || String.IsNullOrWhiteSpace(userModel.LastName))
            {
                return OperationOutput<ApplicationUser>.Failed("Arguments Invalid");
            }

            try
            {
                
                if(dbContext.Users.Any(u => u.Email.Equals(userModel.Email)))
                {
                    return OperationOutput<ApplicationUser>.Failed("User Exists");
                }

                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Email = userModel.Email,
                    PhoneNumber = userModel.PhoneNumber,
                    PasswordHash = Encoding.UTF8.GetString(MySHA256.ComputeHash(Encoding.UTF8.GetBytes(userModel.Password + "MySecret")))
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                return OperationOutput<ApplicationUser>.Success(user);
            }
            catch (Exception ex)
            {
                return OperationOutput<ApplicationUser>.Failed("Internal Server Error",ex.Message);
            }
        }

        /// <summary>
        /// Check if the password is correct
        /// </summary>
        /// <param name="username">username of the user</param>
        /// <param name="password"> password of the user</param>
        /// <returns>whether the password is correct or not</returns>
        public OperationResult CheckPassword(ApplicationUser user, string password)
        {
            if(user == null || String.IsNullOrWhiteSpace(password))
            {
                return OperationResult.Failed("Arguments Null");
            }

            try
            {
                var passwordHash = Encoding.UTF8.GetString(MySHA256.ComputeHash(Encoding.UTF8.GetBytes(password + "MySecret")));

                if (!passwordHash.Equals(user.PasswordHash))
                {
                    return OperationResult.Failed("Incorrect Passoword");
                }

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failed("Internal Server Error",ex.Message);
            }
        }
    }
}
