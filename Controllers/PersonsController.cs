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
        private readonly IOptions<PersonOptions> _personOptions;
        public PersonsController(IOptions<PersonOptions> personOptions)
        {
            _personOptions = personOptions;
        }

        [HttpGet]
        public IActionResult GetPersonOptions()
        {
            PersonOptions personOptions;
            try
            {
                personOptions = _personOptions.Value;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            // catch (OptionsValidationException optValEx)
            // {
            //     return BadRequest(optValEx.Message);
            // }

            return Ok(_personOptions.Value);
        }
    }
}