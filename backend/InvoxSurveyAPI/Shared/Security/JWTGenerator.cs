using Microsoft.Extensions.Options;
using Shared.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Shared.Enums;

namespace Shared.Security
{
    public interface IJWTGenerator
    {
        string GenerateTokenAdmin(BasicSessionModel model);
        string GenerateTokenInterviewer(BasicSessionModel model);
    }

    public class JWTGenerator : IJWTGenerator
    {
        private readonly JWTConfig _jwtconfig;

        public JWTGenerator(IOptions<JWTConfig> jwtConfig)
        {
            _jwtconfig = jwtConfig.Value ?? throw new ArgumentNullException(nameof(jwtConfig));
        }

        public string GenerateTokenAdmin(BasicSessionModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtconfig.SecretKey);
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, model.Name),
                    new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()),
                    new Claim(ClaimTypes.Role, UserRole.Admin.ToString()),
                    new Claim("Provider", model.Provider)
                }),
                NotBefore = now,
                Expires = DateTime.UtcNow.AddMinutes(_jwtconfig.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtconfig.Issuer,
                Audience = _jwtconfig.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateTokenInterviewer(BasicSessionModel model)
        {
            throw new NotImplementedException();
        }
    }
}
