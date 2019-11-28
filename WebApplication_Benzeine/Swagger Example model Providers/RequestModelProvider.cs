
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication_Benzeine.Models.RequestDTO;

namespace WebApplication_Benzeine.Swagger_Example_model_Providers
{
    public class RequestModelProvider : IExamplesProvider<RequestModel>
    {
        RequestModel IExamplesProvider<RequestModel>.GetExamples()
        {
            return new RequestModel()
            {
                Email = "john.lennon@beatles.com",
                Password = "Red Underpants 13!"
            };
        }
    }
}
