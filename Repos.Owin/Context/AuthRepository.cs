using Repos.Owin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repos.Owin.Context
{
    public class AuthRepository : IDisposable
    {

        private AuthContext _ctx;

        public AuthRepository()
        {
            _ctx = new AuthContext();
        }


        public string Findticket(int userid)
        {
            var ticket = _ctx
                            .RefreshTokens
                            .Find(userid)
                            ?.ProtectedTicket;

            return ticket;

        }

        public int AddRefreshToken(RefreshToken token,int userid)
        {
            RefreshToken deltoken = _ctx.RefreshTokens.Find(userid); 

            if (deltoken != null)
                _ctx.RefreshTokens.Remove(deltoken);

            _ctx.RefreshTokens.Add(token);
            return _ctx.SaveChanges();

        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
