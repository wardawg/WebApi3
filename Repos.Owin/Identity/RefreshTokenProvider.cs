using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Repos.Owin.Models;
using Repos.Owin.Entities;
using Repos.Owin.Context;

namespace WebApi.Identity
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {

            var guid = Guid.NewGuid().ToString();

            using (AuthRepository _repo = new AuthRepository())
            {

                // copy all properties and set the desired lifetime of refresh token  
                var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
                {
                    IssuedUtc = context.Ticket.Properties.IssuedUtc,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                };

                var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

                _refreshTokens.TryAdd(guid, refreshTokenTicket);
                
                var client = context.OwinContext.Get<ClientRefInfo>("as:userinfo");
                var usrid = Convert.ToInt32(context.OwinContext.Get<string>("as:UserId"));

                var token = new RefreshToken()
                {
                    Id = guid
                    ,ClientId = client.ExtClientId
                    ,UserId = usrid
                    ,IssuedUtc = DateTime.Now // UtcNow,
                                              //   ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                    ,ProtectedTicket = context.SerializeTicket()
                };

                _repo.AddRefreshToken(token, usrid);


                context.SetToken(guid);

            }
            // consider storing only the hash of the handle  
            

            return Task.FromResult<object>(null);
        }


        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }


        Task IAuthenticationTokenProvider.ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            /*
             *   var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            */
                
            // context.DeserializeTicket(context.Token);
          //  AuthenticationTicket ticket;
            string header = context.OwinContext.Request.Headers["Authorization"];

            var userid = Convert.ToInt32(context.OwinContext.Get<string>("as:user_id"));

            using (AuthRepository _repo = new AuthRepository())
            {
                var protectedTicket = _repo.Findticket(userid);

                if (protectedTicket != null)
                {
                        context.DeserializeTicket(protectedTicket);
                    }
               // }
            }
            return Task.FromResult<object>(null);
        }
    }
}