using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt
{
    public class JwtService
    {
        private string secureKey = "this is a very secure key";

        public object JWTheader { get; private set; }

        public string Generate(int Id)
        {
            var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var Credentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(Credentials);

            var payload = new JwtPayload(Id.ToString(), null, null, null, DateTime.Today.AddDays(1));
            var securityToken = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public JwtSecurityToken Varify(string Jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);

            tokenHandler.ValidateToken(Jwt, new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken) validatedToken;
        }
    }
}
