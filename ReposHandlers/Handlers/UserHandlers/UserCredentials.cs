using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repos.DomainModel.Interface.Atrributes.DomainAttributes;
using Repos.DomainModel.Interface.Interfaces;
using ReposCore.Caching;
using ReposData.Repository;
using ReposDomain.Domain;
using ReposServiceConfigurations.ServiceTypes.Base;
using ReposServiceConfigures.ServiceTypes.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ReposDomain.Handlers.Handlers.UserHandlers
{
    public interface IUserCredentials 
        : IGenericHandler
    {
        ClaimsIdentity GetUserInfo(string UserName, string Password);
        ClaimsIdentity GetUserInfo(IEnumerable<ClientRefInfo> ClientRefInfo,string UserName, string Password);
        
        IEnumerable<ClientRefInfo> GetClients();
    }

    [DomainNoBindAttribute]
    public class UserCredentials
        : ServiceGenericHandler<User>
        , IUserCredentials
    {
        private IRepository<ClientRefInfo>  _clientRepos;
        private IRepository<UserRole>       _UserRoleRepos;

        public UserCredentials(IRepository<User> repos
                              ,IRepository<ClientRefInfo> clientRepos
                              ,IRepository<UserRole> userRole
                              , ICacheService cache

                              )
            : base(repos,cache){

            _UserRoleRepos = userRole;
            _clientRepos   = clientRepos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientRefInfo> GetClients()
        {
            var cacheKey = string.Concat(this.GetType().Name, "GetClients");

            return HandlerCache.Get<List<ClientRefInfo>>(cacheKey,  ()=>
             {
                 return _clientRepos
                            .TableNoTracking
                            .ToList();
             });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public ClaimsIdentity GetUserInfo(IEnumerable<ClientRefInfo> clientInfo
                                          ,string UserName
                                          ,string Password)
        {

            return Get(UserName,Password,clientInfo);
        }

        private ClaimsIdentity Get(string UserName, string Password, IEnumerable<ClientRefInfo> ClientRefInfo)
        {
            var results = (from usr in ((IRepository<User>)_repos)
                                        .TableNoTracking
                             from cr in ClientRefInfo

                             let client = usr.Client

                             where cr.Id == client.Id
                                   && usr.UserName == UserName

                             select new
                             {
                                 usr.Id
                                 , usr.PasswordHash
                                 , usr.PasswordSalt
                                 , usr.ClientId
                                 , cr.ExtClientId
                                 , usr.UserName
                                 , roles = from ur in _UserRoleRepos
                                                    .TableNoTracking
                                          let role = ur.Role
                                          where ur.Id ==  usr.Id
                                          select role
                           }).FirstOrDefault();

         

            var identity = default(ClaimsIdentity);

            if (results != null)
                if (VerifyPasswordHash(Password, results.PasswordHash, results.PasswordSalt))
                {
                    identity = new ClaimsIdentity("JWT");
                    identity.AddClaim(new Claim("Name", results.UserName));
                    identity.AddClaim(new Claim("UserId", results.Id.ToString()));
                    identity.AddClaim(new Claim("ClientId", results.ExtClientId));
                    
                    dynamic json;

                    char quote = (char)34;

                    json = new JObject();
                    json.roles = new JArray() as dynamic;

                    



                    var sb = new StringBuilder();
                    var comma = string.Empty;
                    sb.Append("{ ");
                    sb.Append("roles: [ ");
                    foreach (var role in results.roles)
                    {
                        sb.Append(String.Format(" {0}{{Id:" + "" + "{1}" + "" + ",RoleName:" + quote + "{2}" + quote + "}}", comma, role.Id, role.RoleName));
                        comma = ",";
                        //dynamic varrole = new JObject();
                        //varrole.Id = role.Id;
                        //varrole.RoleName = role.RoleName;
                        //json.roles.Add(varrole);
                    }

                    sb.Append(" ]");
                    sb.Append(" }");
                              
                    identity.AddClaim(new Claim("Roles", sb.ToString().Replace("\\","")));

                                                            
                    
                    //.ToString()
                    //                                        .Replace("\r\n", "")
                    //                                        .Replace("\\", "")));

                    
                }

            return identity;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public ClaimsIdentity GetUserInfo(string UserName, string Password)
        {
            return Get(UserName, Password, _clientRepos.TableNoTracking);
                      
        }


        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
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
