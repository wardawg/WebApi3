using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebApi.App_Start;
using WebApi.Identity;

[assembly: OwinStartup(typeof(Startup))]

namespace WebApi.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            var issuer = "http://localhost/";
            var setting_secret = "IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw";

            var secret = Microsoft
                        .Owin
                        .Security
                        .DataHandler
                        .Encoder
                        .TextEncodings
                        .Base64Url
                        .Decode(setting_secret);
                        
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { "Any" },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                {
                    new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                  // ,new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                }
            });

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat(issuer),
                RefreshTokenProvider = new RefreshTokenProvider()
            });
        }

    }
}