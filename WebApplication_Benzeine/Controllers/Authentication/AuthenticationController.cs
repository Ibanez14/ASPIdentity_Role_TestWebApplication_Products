using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebApplication_Benzeine.Models.RequestDTO;
using WebApplication_Benzeine.Models.ResponseDTO;
using WebApplication_Benzeine.Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication_Benzeine.Controller
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        readonly IAuthenticationService authService;
        public IdentityController(IAuthenticationService authService)=>
                    this.authService = authService;


        /// <summary>
        /// Receive requestModel and if succeessfully registered then returns a JWT Token, otherwise Error list. 
        /// </summary>
        /// <param name="request">Request model parameter</param>
        /// <response code="200">Success. Registration succeeded. This status returns JWT token </response>
        /// <response code="400">Bad Request. Registration failed. This status may be returned in case there is already exist such a user, or requestModel wasn't valid</response>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<AuthenticationResult>> Register([FromBody] RequestModel request)
        {
            var authResult = await authService.RegisterAsync(request.Email, request.Password);

            if (!authResult.Success)
                return BadRequest(authResult);
            else
                return Ok(authResult);
        }


        /// <summary>
        /// Receive requestModel and if succeessfully logged in then returns a JWT Token, otherwise Error list. 
        /// </summary>
        /// <param name="request">Request model parameter</param>
        /// <response code="200">Success. Login succeeded. This status returns JWT token </response>
        /// <response code="400">Bad Request. Registration failed. This status may be returned Error-related information</response>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<AuthenticationResult>> Login([FromBody] RequestModel request)
        {
            var authResult = await authService.LoginAsync(request.Email, request.Password);

            if (!authResult.Success)
                return BadRequest(authResult);
            else
                return Ok(authResult);
        }
    }
}
