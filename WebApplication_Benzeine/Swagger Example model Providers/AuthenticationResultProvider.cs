
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using System.Linq;
using WebApplication_Benzeine.Models.ResponseDTO;

namespace WebApplication_Benzeine.Swagger_Example_model_Providers
{
    public class AuthenticationResultSuccess : AuthenticationResult
    {
        public AuthenticationResultSuccess(string jwt):base(jwt, true)
        {

        }
    }

    public class AuthenticaionResultFail : AuthenticationResult
    {
        public AuthenticaionResultFail(params string [] errors) : base(errors)
        {

        }
    }


    public class AuthenticationResultSuccessProvider : IExamplesProvider<AuthenticationResultSuccess>
    {
        AuthenticationResultSuccess IExamplesProvider<AuthenticationResultSuccess>.GetExamples()=>
            new AuthenticationResultSuccess("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"); 
        
    }
    public class AuthenticationResultFailProvider : IExamplesProvider<AuthenticaionResultFail>
    {
        AuthenticaionResultFail IExamplesProvider<AuthenticaionResultFail>.GetExamples()=>
            new AuthenticaionResultFail("Password must have at least 6 charactes", "Provide correct email address");
        
    }


}
