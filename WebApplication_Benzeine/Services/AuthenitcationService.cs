using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication_Benzeine.Models.ResponseDTO;
using WebApplication_Benzeine.Options;

namespace WebApplication_Benzeine.Services
{
    /// <summary>
    /// Service is responsible for authentication users and generate JWT Tokens
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        UserManager<IdentityUser> userManager;
        JwtOptions jwtOptions;

        public AuthenticationService(UserManager<IdentityUser> userManager,
                                   IOptionsSnapshot<JwtOptions> jwtOptions)
        {
            this.userManager = userManager;
            this.jwtOptions = jwtOptions.Value;
        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
                return AuthenticationResult.FailResult("Sorry, user already exists. Don't do that again");

            // if user doesnt exists, create a new user
            var newUser = new IdentityUser() { Email = email, UserName = email };
            var identityResult = await userManager.CreateAsync(newUser, password);

            if (email.EndsWith("benzeine.com"))
                await userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, "Developer"));

            if (!identityResult.Succeeded)
                return AuthenticationResult.FailResult(identityResult.Errors.Select(e => e.Description));
            else
                return await GenerateAuthenticationResultAsync(newUser);
        }



        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return AuthenticationResult.FailResult("Sorry, user doesn't exist. Don't do that again");

            // if passwords are mattched we generate JWT
            if (await userManager.CheckPasswordAsync(user, password))
                return await GenerateAuthenticationResultAsync(user);
            else
                return AuthenticationResult.FailResult("Sorry, Passwords don't match, please dry your fingers");
        }




        private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(IdentityUser user)
        {
            // return UserClaims with JwtClaims
            var claims = await GenerateJwtClaimsAsync(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(jwtOptions.TokenLifeTime),
                SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
                                                    algorithm: SecurityAlgorithms.HmacSha256),
                Issuer = "Benzeine",
                IssuedAt = DateTime.UtcNow
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(tokenDescriptor);
            string jwt = handler.WriteToken(token);

            return AuthenticationResult.SuccessResult(jwt);

            #region Local Method

            // Local method for generating JWTClaims
            async Task<IEnumerable<Claim>> GenerateJwtClaimsAsync(IdentityUser user)
            {
                var jwtClaims = new List<Claim>
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Id", user.Id)
            };
                var userClaims = await userManager.GetClaimsAsync(user);
                // Return JWT and User Claims in one collection
                return jwtClaims.Concat(userClaims);
            }
            #endregion
        }


    }
}






