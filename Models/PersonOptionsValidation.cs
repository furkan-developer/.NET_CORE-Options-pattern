using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace OptionsPattern.Models
{
    public class PersonOptionsValidation : IValidateOptions<PersonOptions>
    {
        private readonly PersonOptions _personOptions;

        public PersonOptionsValidation(IConfiguration configuration)
        {
            _personOptions = configuration.GetSection(PersonOptions.Person).Get<PersonOptions>();
        }

        public ValidateOptionsResult Validate(string name, PersonOptions options)
        {
            string errorMessages = null;
           
            // use _personOptions for validating PersonOptions

            if(errorMessages != null)
                return ValidateOptionsResult.Fail(errorMessages);

            return ValidateOptionsResult.Success;
        }
    }
}