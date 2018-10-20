using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using System;
using System.Linq;

namespace ReposServices.UsersServices
{

    public interface IUserService
        : IBaseService<User>
    {
        User Authenticate(string username, string password);
        User Create(User user, string password);
    }
    public class UserService
         : BaseService<User>
         , IUserService
    {
      
       public UserService(IRepository<User> UserRepository)  
            :base(UserRepository,null, null,null){

        }


        public User Create(User user, string password)
        {
            // validation
            // if (string.IsNullOrWhiteSpace(password))
            //     throw new AppException("Password is required");

            if (_Repos.TableNoTracking.Any(x => x.UserName == user.UserName))
                throw new Exception("Username " + user.UserName + " is already taken");



           // if (_context.Users.Any(x => x.Username == user.Username))
           //     throw new AppException("Username " + user.Username + " is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ClientId = 1; 

            _Repos.Add(user);
      
            return user;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _Repos.Where(w => w.UserName == username).FirstOrDefault(); //     //  .SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;
            
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

    }
}
