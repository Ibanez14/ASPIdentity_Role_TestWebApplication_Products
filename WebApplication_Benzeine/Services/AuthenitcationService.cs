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
using WebApplication_Benzeine.Data.Models.Domain;
using WebApplication_Benzeine.Helpers;
using WebApplication_Benzeine.Models.ResponseDTO;
using WebApplication_Benzeine.Options;

namespace WebApplication_Benzeine.Services
{
    /// <summary>
    /// Service is responsible for authentication users and generate JWT Tokens
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        UserManager<ApplicationUser> userManager;
        JwtOptions jwtOptions;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
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
            var newUser = new ApplicationUser() { Email = email, UserName = email };
            var identityResult = await userManager.CreateAsync(newUser, password);

            if (!identityResult.Succeeded)
                return AuthenticationResult.FailResult(identityResult.Errors.Select(e => e.Description));
            
                await userManager.AddToRoleAsync(newUser, "User");
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




        private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(ApplicationUser user)
        {
            // return UserClaims
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

            return AuthenticationResult.SuccessResult(jwt, claims.ToDictionary());

            #region Local Method

            // Local method for generating JWTClaims
            async Task<IEnumerable<Claim>> GenerateJwtClaimsAsync(ApplicationUser _user)
            {
                var userRoles = await userManager.GetRolesAsync(_user);
                var role = userRoles.FirstOrDefault();

                var jwtClaims = new List<Claim>
                {
                    new Claim("Email", _user.Email),
                    new Claim("UserID", _user.Id),
                    new Claim("Role", role)
                };

                return jwtClaims;
            }
            #endregion
        }


    }
}






