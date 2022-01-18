using Expenses.Core.Abstractions;
using Expenses.Core.CustomExeptions;
using Expenses.Core.DTO;
using Expenses.Core.Utilities;
using Expenses.DB;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Core
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticatedUser> ExternalSignIn(User user)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(localUser => localUser.ExternalId == user.ExternalId
                && localUser.ExternalType == user.ExternalType);

            if (dbUser == null) 
            {
                user.Username = CreateUniqueUsernameFromEmail(user.Email);
                return await SignUp(user);
            }

            return new AuthenticatedUser
            {
                UserName = dbUser.Username,
                Token = JWTGenerator.GenerateAuthToken(dbUser.Username),
            };
            
        }

        public async Task<AuthenticatedUser> SignIn(User user)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);
            if (dbUser == null ||
                dbUser.Password == null ||
                _passwordHasher.VerifyHashedPassword(dbUser.Password, user.Password) == PasswordVerificationResult.Failed) 
            {
                throw new InvalidCredentialsExeption("Username or password incorrect");
            }
            return new AuthenticatedUser
            {
                UserName = user.Username,
                Token = JWTGenerator.GenerateAuthToken(user.Username),
            };
        }

        public async Task<AuthenticatedUser> SignUp(User user)
        {
            var checkUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(user.Username));

            if (checkUser != null) 
            {
                throw new UsernameExistsExeption("User with the same name already exists");
            }

            if (!string.IsNullOrEmpty(user.Password)) 
            {
                user.Password = _passwordHasher.HashPassword(user.Password);
            }

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new AuthenticatedUser
            {
                UserName = user.Username,
                Token = JWTGenerator.GenerateAuthToken(user.Username),
            };
        }
        
        private string CreateUniqueUsernameFromEmail(string email)
        {
            var emailSplit = email.Split('@').First();
            var random = new Random();
            var username = emailSplit;
            while(_context.Users.Any(user => user.Username.Equals(username))) 
            {
                username = emailSplit + random.Next(10000000);
            }
            return username;
        }
    }

    
}
