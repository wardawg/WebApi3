using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using ReposDomain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using ReposDomain.Handlers.Handlers.Common;
using ReposServiceConfigures.ServiceTypes.Handlers;
using ReposDomain.Handlers.Handlers.UserHandlers;
using ReposCore.Caching;
using ReposCore.Infrastructure;
using Repos.Owin.Models;

namespace WebApi.Identity
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private ICacheManager _CacheManager = EngineContext
                                      .Current
                                      .ContainerManager
                                      .Resolve<ICacheManager>("repos_cache_static");


        IEnumerable<Repos.Owin.Models.ClientRefInfo> getClients()
        {
            var _handler = new ServiceHandlerFactory();

            return _handler
                    .Using<IUserCredentials>()
                    .GetClients()
                    .Select(s => new Repos.Owin.Models.ClientRefInfo()
                    {
                        AssmPrefix = s.AssmPrefix
                        ,ExtClientId = s.ExtClientId
                        ,ClientId = s.Id
                        ,ClientKey = s.ClientKey
                    });
                    
        }
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            //var user = _userService.Authenticate(userModel.UserName, userModel.Password);

            var _handler = new ServiceHandlerFactory();

            //var clients = _CacheManager.Get<IEnumerable<ClientRefInfo>>("Client Info", () => getClients());


            var UserInfo = _handler
                    .Using<IUserCredentials>()
                    .GetUserInfo(context.UserName, context.Password);

            if (UserInfo == null)
                context.Rejected();
            else
            {
                context.OwinContext.Set<string>("as:UserId", UserInfo.FindFirst("UserId").Value);
                var ticket = new AuthenticationTicket(UserInfo, new AuthenticationProperties());
                context.Validated(ticket);
            }
            
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            
                string clientId;
                string clientSecret;
                if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                    context.TryGetFormCredentials(out clientId, out clientSecret))
                {

                var clients = _CacheManager.Get<IEnumerable<Repos.Owin.Models.ClientRefInfo>>("Client Info", () => getClients());

                var client = clients.Where(w => w.ExtClientId == clientId
                                                && w.ClientKey == clientSecret)
                                    .FirstOrDefault();

                if (client != null)
                {
                    var user_id = context.Parameters.Get("userid");

                    context.OwinContext.Set<string>("as:user_id", user_id);
                    context.OwinContext.Set<string>("as:client_id", clientId);
                    context.OwinContext.Set<Repos.Owin.Models.ClientRefInfo>("as:userinfo", client);
                    context.Validated();
                }

            }
            
            return Task.FromResult<object>(null);
        }

        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, IdentityUser user)
        {
            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("ClientId", context.OwinContext.Get<string>("as:client_id")));
            
            return identity;
        }
    }
}