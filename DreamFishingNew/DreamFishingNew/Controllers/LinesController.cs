using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Lines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class LinesController: Controller
    {
        private ApplicationDbContext data;

        public LinesController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery]AllLinesQueryModel query)
        {
            var linesQuery = data.Lines
                .Include<Line>("Brand")
                .ToList();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                linesQuery = data.Lines
                    .Where(x => x.Brand.Name.ToLower() == query.Brand)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                linesQuery = linesQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();
            }

            linesQuery = query.Sorting switch
            {
                LineSorting.MinPrice => linesQuery.OrderBy(x => x.Price).ToList(),
                LineSorting.MaxPrice => linesQuery.OrderByDescending(x => x.Price).ToList(),
                LineSorting.BrandAndModel => linesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => linesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            var lines = linesQuery
                .Skip((query.currentPage -1) * AllLinesQueryModel.LinesPerPage)
                .Take(AllLinesQueryModel.LinesPerPage)
                .Select(x => new LineListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            


            var lineBrands = data
                .Baits
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();


            var model = new AllLinesQueryModel
            {
                Brand = query.Brand,
                Brands = lineBrands,
                Lines = lines,
                Sorting = query.Sorting,
                SearchTerm = query.SearchTerm,
            };

            return View(model);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddLineFormModel line)
        {
             var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == line.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(line.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(line);
            }


            var currentLine = new Line
            {
                Model = line.Model,
                BrandId = brand.Id,
                Image = line.Image,
                Price = line.Price,
                Length = line.Length,
                Weight = line.Weight,
                Size = line.Size,
                Description = line.Description,
                Quantity = line.Quantity
            };

            data.Lines.Add(currentLine);
            data.SaveChanges();

            return RedirectToAction("Add", "Lines");
        }
    }
}
