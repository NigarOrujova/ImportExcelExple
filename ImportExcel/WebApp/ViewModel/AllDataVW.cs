using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.ViewModel
{
    public class AllDataVW
    {
        public IQueryable<Countries> Countries { get; set; }
        public IQueryable<City> Cities { get; set; }
    }
}
