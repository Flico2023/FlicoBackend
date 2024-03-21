using FlicoProject.DtoLayer;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.BusinessLayer.Concrete
{
    public class JwtManager
    {
        public static TokenResponse GenerateToken(User user) { 
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FlicoIsAwesomeFlicoIsAwesomeFlicoIsAwesome"));
            var SigninCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var expireDate = DateTime.UtcNow.AddDays(90);
            JwtSecurityToken token = new JwtSecurityToken(issuer: "http://localhost",audience: "http://localhost",claims:claims,
                notBefore:DateTime.UtcNow,expires:expireDate,signingCredentials:SigninCredentials);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return new TokenResponse(tokenHandler.WriteToken(token), expireDate);
        }
    }
}
