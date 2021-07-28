using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Baits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class BaitsController:Controller
    {
        private ApplicationDbContext data;

        public BaitsController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery]AllBaitsQueryModel query)
        {
            var baitsQuery = data.Baits
                .Include<Bait>("Brand")
                .ToList();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                baitsQuery = data.Baits
                    .Where(x => x.Brand.Name.ToLower() == query.Brand)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                baitsQuery = baitsQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();
            }

            baitsQuery = query.Sorting switch
            {
                BaitSorting.MinPrice => baitsQuery.OrderBy(x => x.Price).ToList(),
                BaitSorting.MaxPrice => baitsQuery.OrderByDescending(x => x.Price).ToList(),
                BaitSorting.BrandAndModel => baitsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => baitsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            var baits = baitsQuery
                .Skip((query.currentPage -1) * AllBaitsQueryModel.BaitsPerPage)
                .Take(AllBaitsQueryModel.BaitsPerPage)
                .Select(x => new BaitListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            


            var baitBrands = data
                .Baits
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();


            var model = new AllBaitsQueryModel
            {
                Brand = query.Brand,
                Brands = baitBrands,
                Baits = baits,
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
        public IActionResult Add(AddBaitFormModel bait)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == bait.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(bait.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(bait);
            }


            var currentBait = new Bait
            {
                Model = bait.Model,
                BrandId = brand.Id,
                Image = bait.Image,
                Price = bait.Price,
                Length = bait.Length,
                Weight = bait.Weight,
                Type = bait.Type,
                Description = bait.Description,
                Quantity = bait.Quantity
            };

            data.Baits.Add(currentBait);
            data.SaveChanges();

            return RedirectToAction("Add", "Baits");
        }

        public IActionResult Details(int id)
        {

            var bait = data
                .Baits
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);


            var model = new BaitDetailsViewModel
            {
                Id = bait.Id,
                Model = bait.Model,
                Brand = bait.Brand.Name,
                Image = bait.Image,
                Length = bait.Length,
                Type = bait.Type,
                Weight = bait.Weight,
                Description = bait.Description,
                Price = bait.Price,
                Quantity = bait.Quantity
            };

            return View(model);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = data.Baits
                .Where(x => x.Id == id)
                .Select(x => new AddBaitFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                Length = x.Length,
                Type = x.Type,
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
        public IActionResult Edit(int id, AddBaitFormModel item)
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
                .Baits
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            bait.Model = item.Model;
            bait.Brand.Name = item.Brand;
            bait.Length = item.Length;
            bait.Description = item.Description;
            bait.Image = item.Image;
            bait.Type = item.Type;
            bait.Price = item.Price;
            bait.Quantity = item.Quantity;
            bait.Weight = item.Weight;

            data.SaveChanges();

            return RedirectToAction("All", "Baits");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var bait = data
                .Baits
                .Where(x => x.Id == id)
                .FirstOrDefault();

            data.Baits.Remove(bait);
            data.SaveChanges();

            return RedirectToAction("All", "Baits");
        }
    }
}
