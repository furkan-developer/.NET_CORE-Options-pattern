using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OptionsPattern.Models
{
    public class PersonOptions
    {
        public const string Person = "Person";

        [MaxLength(10,ErrorMessage = "Name's max lenght should be 10")]
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}