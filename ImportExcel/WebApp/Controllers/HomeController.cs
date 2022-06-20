using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DAL;
using WebApp.Models;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger,AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int page=1,int take= 10)
        {
            List<Countries> countries = await _context.Countries
                                                     .Skip((page - 1) * take)
                                                     .Take(take)
                                                     .ToListAsync();
            Paginate<Countries> result = new Paginate<Countries>();
            result.Items = countries;
            result.CurrentPage = page;
            result.AllPageCount = await getPageCount(10);
            return View(result);
        }

        public async Task<int> getPageCount(int take)
        {
            List<Countries> countries = await _context.Countries.ToListAsync();
            int countrCount = countries.Count;
            return (int)Math.Ceiling(((decimal)countrCount / take));
        }

        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row = 2; row < rowcount+1; row++)
                    {
                        Countries countries = new Countries()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CountryName = worksheet.Cells[row, 2].Value==null? string.Empty: worksheet.Cells[row, 2].Value.ToString().Trim(),
                            TwoCharCountryCode = worksheet.Cells[row, 3].Value==null? string.Empty: worksheet.Cells[row, 3].Value.ToString().Trim(),
                            ThreeCharCountryCode = worksheet.Cells[row, 4].Value== null ? string.Empty : worksheet.Cells[row, 4].Value.ToString().Trim()
                        };
                        if (await _context.Countries.AnyAsync(
                        p => p.CountryName == countries.ThreeCharCountryCode &&
                        p.ThreeCharCountryCode == countries.ThreeCharCountryCode &&
                        p.TwoCharCountryCode == countries.TwoCharCountryCode))
                        {
                            continue;
                        }
                        await _context.Countries.AddAsync(countries);
                    }
                }
                 await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index","Home");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
