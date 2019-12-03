using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication_Benzeine.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication_Benzeine.Controllers
{
    [Route("api/[controller]")]
    public class TestEmailController : ControllerBase
    {

        IEmailSender emailSender;

        public TestEmailController(IEmailSender emailSender)
        {
            this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }


        [HttpGet]
        public async Task<IActionResult>SendEmail()
        {
            var response = await emailSender.SendEmailAsync("sov.shamil@gmail.com", "Ay can, ay can", "Hola ?");

            return Ok("Mail is sent");
        }


    }
}
