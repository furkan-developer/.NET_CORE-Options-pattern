using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OptionsPattern.Models;

namespace OptionsPattern.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PersonsController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult GetPersonOptions()
        {
            // var personOptions = new PersonOptions();
            // _config.GetSection(PersonOptions.Person).Bind(personOptions);

            var personOptions = _config.GetSection(PersonOptions.Person).Get<PersonOptions>();

            return Ok(personOptions);
        }
    }
}