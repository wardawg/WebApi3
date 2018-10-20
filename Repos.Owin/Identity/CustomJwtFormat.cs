using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Text;
using Thinktecture.IdentityModel.Tokens;

namespace WebApi.Identity
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private static readonly byte[] _secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");
        private readonly string _issuer = "http://localhost/";

        
        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

         
            var credentials = new HmacSigningCredentials(_secret);
                        
            //  Finally create a Token
            //var header = new JwtHeader(credentials);
            
            var handler = new JwtSecurityTokenHandler();

           
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            return new JwtSecurityTokenHandler()
                        .WriteToken(new JwtSecurityToken(_issuer,
                                                           "Any",
                                                           data.Identity.Claims,
                                                           issued.Value.UtcDateTime,
                                                           expires.Value.UtcDateTime
                                                           , credentials
                                                            ));
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}