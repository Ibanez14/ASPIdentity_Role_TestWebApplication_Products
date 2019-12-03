#define TESTINGIDENTITY

#if RELEASE
#undef TESTINGIDENTITY
#endif

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
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
        UserManager<AppUser> userManager;
        JwtOptions jwtOptions;


        public AuthenticationService(UserManager<AppUser> userManager,
                                   IOptionsSnapshot<JwtOptions> jwtOptions)
        {
            this.userManager = userManager;
            this.jwtOptions = jwtOptions.Value;
        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
           
            // if user doesnt exists, create a new user
            var newUser = new AppUser() { Email = email, UserName = email};


#if TESTINGIDENTITY

            //
            // Email Confirmation
            //

            string cToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
            string b64token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(cToken));

            var callbackUrl = new Url($"Callback/url/{b64token}");
            
            // Email Service that send email
            // email_service.SendEmailAsync(callbackUrl);


            if(userManager.Options.SignIn.RequireConfirmedEmail)
                // return to some page to wait until he confirm his email



            // just testing Identity
            if (newUser.Email.EndsWith("benzeine.com"))
            {
                newUser.AccessFailedCount = 100;
                newUser.EmailConfirmed = true;
                newUser.LockoutEnabled = false;

                //newUser.LockoutEnd = DateTime.Today.AddDays(30);
            }








#endif

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




        private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(AppUser user)
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
            async Task<IEnumerable<Claim>> GenerateJwtClaimsAsync(AppUser _user)
            {
                var userRoles = await userManager.GetRolesAsync(_user);
                var role = userRoles.FirstOrDefault();

                var jwtClaims = new List<Claim>
                {
                    new Claim("Email", _user.Email),
                    new Claim("UserID", _user.Id.ToString()),
                    new Claim("Role", role)
                };

                return jwtClaims;
            }
            #endregion
        }


    }
}






