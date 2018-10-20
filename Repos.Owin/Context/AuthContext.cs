using Repos.Owin.Entities;
using Repos.Owin.Models;
using System.Data.Entity;

namespace Repos.Owin.Context
{
    public class AuthContext :DbContext
    {
        public AuthContext()
            : base("AuthContext")
        {
            base.Configuration.AutoDetectChangesEnabled = false;
            base.Configuration.ValidateOnSaveEnabled = false;
        }

        public DbSet<ClientRefInfo> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}