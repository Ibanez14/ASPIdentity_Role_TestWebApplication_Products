using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication_Benzeine.Controller
{
    /// <summary>
    /// This controller return wifi password to Users that have email with benzeine.com domain
    /// </summary>
    [Authorize(policy: "OnlyBezeine")]
    [Route("api/[controller]/[action]")]
    public class WifiController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPassword() => 
            Ok(new { wifi= "BENZEINE", password = "benzeine2019" });


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Test() =>
            Ok("Testing");

    }

}
