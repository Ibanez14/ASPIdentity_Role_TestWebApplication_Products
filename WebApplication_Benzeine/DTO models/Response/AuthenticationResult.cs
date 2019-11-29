using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication_Benzeine.Models.ResponseDTO
{
    /// <summary>
    /// Response model that is used in primarily AuthenticationController
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Whether authentication succeeded or not
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error list in case if authentication failed
        /// </summary>
        public IEnumerable<string> Errors { get; set; }

        /// <summary>
        /// User-related claims
        /// </summary>
        public IDictionary<string, string> UserClaims { get; set; }
        
        #region CTOR-s

        public AuthenticationResult(IEnumerable<string> errors) =>
            (Errors) = (errors);

        public AuthenticationResult(string token, bool success, IDictionary<string, string>  claims) =>
            (Token, Success, UserClaims) = (token, success, claims);

        #endregion

        public static AuthenticationResult FailResult(params string[] errors)
            => new AuthenticationResult(errors);

        public static AuthenticationResult FailResult(IEnumerable<string> errors)
            => new AuthenticationResult(errors);

        public static AuthenticationResult SuccessResult(string jwt, IDictionary<string, string> claims) =>
            new AuthenticationResult(jwt, true, claims);

    }



}
