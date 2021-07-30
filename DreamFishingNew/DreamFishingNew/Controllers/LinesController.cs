using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Lines;
using DreamFishingNew.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

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

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
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

        public IActionResult Details(int id)
        {

            var line = data
                .Lines
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);


            var model = new LineDetailsViewModel
            {
                Id = line.Id,
                Model = line.Model,
                Brand = line.Brand.Name,
                Image = line.Image,
                Length = line.Length,
                Size = line.Size,
                Weight = line.Weight,
                Description = line.Description,
                Price = line.Price,
                Quantity = line.Quantity
            };

            return View(model);
        }

        [Authorize]
        public IActionResult AddtoCart(int id, string userId)
        {
            //var currUser = data.Users.Where(x => x.Id == userId).FirstOrDefault();
            var currLine = data
                .Lines
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            currLine.Quantity--;

            if (currLine.Quantity < 0)
            {
                currLine.Quantity = 0;
            }

            var bagModel = new AddtoCartViewModel
            {
                Model = currLine.Model,
                Brand = currLine.Brand.Name,
                Image = currLine.Image, 
                Quantity = currLine.Quantity
            };

            data.SaveChanges();
            return View(bagModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = data.Lines
                .Where(x => x.Id == id)
                .Select(x => new AddLineFormModel 
                {              
                Model = x.Model,
                Brand = x.Brand.Name,
                Length = x.Length,
                Size = x.Size,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Price = x.Price,
                Quantity = x.Quantity
                })
                .FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddLineFormModel item)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == item.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(item);
            }


            var bait = data
                .Lines
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            bait.Model = item.Model;
            bait.Brand.Name = item.Brand;
            bait.Length = item.Length;
            bait.Description = item.Description;
            bait.Image = item.Image;
            bait.Size = item.Size;
            bait.Price = item.Price;
            bait.Quantity = item.Quantity;
            bait.Weight = item.Weight;

            data.SaveChanges();

            return RedirectToAction("All", "Lines");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var line = data
                .Lines
                .Where(x => x.Id == id)
                .FirstOrDefault();

            data.Lines.Remove(line);
            data.SaveChanges();

            return RedirectToAction("All", "Lines");
        }
    }
}
