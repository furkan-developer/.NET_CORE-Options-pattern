using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OptionsPattern.Models
{
    public class PersonOptions
    {
        public const string Person = "Person";
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}