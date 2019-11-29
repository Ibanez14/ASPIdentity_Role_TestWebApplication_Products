using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication_Benzeine.Controller.Exception_Handling
{
    /// <summary>
    /// Simple controller for handling Expcetions and punish them
    /// </summary>
    [Route("Error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public ActionResult<JsonResult> Get()
        {
            var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>();
            HttpContext.Response.StatusCode = 500;
            // return a tuple
            return new JsonResult(("Сервер отошел отлить", exceptionHandler.Error.Message));
        }
    }
}
