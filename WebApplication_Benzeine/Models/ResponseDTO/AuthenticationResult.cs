using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_Benzeine.Models.ResponseDTO
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }


        #region CTOR-s

        public AuthenticationResult(IEnumerable<string> errors) =>
            (Errors) = (errors);

        public AuthenticationResult(string token, bool success) =>
            (Token, Success) = (token, success);

        #endregion

        public static AuthenticationResult FailResult(params string[] errors)
            => new AuthenticationResult(errors);

        public static AuthenticationResult FailResult(IEnumerable<string> errors)
            => new AuthenticationResult(errors);

        public static AuthenticationResult SuccessResult(string jwt) =>
            new AuthenticationResult(jwt, true);

    }


}
