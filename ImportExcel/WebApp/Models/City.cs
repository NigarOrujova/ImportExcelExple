using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class City
    {
        public string Id { get; set; }

        public string CountryName { get; set; }

        public string TwoCharCountryCode { get; set; }

        public string ThreeCharCountryCode { get; set; }
        public string CountriesId { get; set; }
        public Countries Countries { get; set; }
    }
}
