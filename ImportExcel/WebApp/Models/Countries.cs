using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Countries
    {
        public string Id { get; set; }

        public string CountryName { get; set; }

        public string TwoCharCountryCode { get; set; }

        public string ThreeCharCountryCode { get; set; }
        public IQueryable<City> Cities { get; set; }
    }
}
