using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorldModel;

namespace comp_584_server
{
    public class JwtHandler(UserManager<WorldModelUser> UserManager, IConfiguration configuration)
    {
        public async Task<JwtSecurityToken> GenerateTokenAsync(WorldModelUser user)
        {

             return new JwtSecurityToken (
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtSettings:ExpiryInMinutes"])),
                claims : await GetClaimsAsync(user),
                signingCredentials : GetSigningCredentials()

            );
            //return await Task.FromResult(new JwtSecurityToken());
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!);
            SymmetricSecurityKey signingkey = new(key);
            return new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256);

        }
        private async Task<List<Claim>> GetClaimsAsync(WorldModelUser user)
        {
            List<Claim> claims = [new Claim(ClaimTypes.Name, user.UserName!)];
            //claims.AddRange((await userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role)));
            foreach(var role in await UserManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

    }
}
