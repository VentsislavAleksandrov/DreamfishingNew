using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Rods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class RodsController: Controller
    {
        private ApplicationDbContext data;

        public RodsController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery] AllRodsQueryModel query)
        {
            var rodsQuery = data.Rods
                .Include<Rod>("Brand")
                .ToList();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                rodsQuery = data.Rods
                    .Where(x => x.Brand.Name.ToLower() == query.Brand)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                rodsQuery = rodsQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Type.ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();
            }

            rodsQuery = query.Sorting switch
            {
                RodSorting.MinPrice => rodsQuery.OrderBy(x => x.Price).ToList(),
                RodSorting.MaxPrice => rodsQuery.OrderByDescending(x => x.Price).ToList(),
                RodSorting.lenght => rodsQuery.OrderByDescending(x => x.Length).ToList(),
                RodSorting.BrandAndModel => rodsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => rodsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            var rods = rodsQuery
                .Skip((query.CurrentPage -1) * AllRodsQueryModel.RodsPerPage)
                .Take(AllRodsQueryModel.RodsPerPage)
                .Select(x => new RodListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                    Type = x.Type
                })
                .ToList();

            


            var rodBrands = data
                .Rods
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();


            var model = new AllRodsQueryModel
            {
                Brand = query.Brand,
                Brands = rodBrands,
                Rods = rods,
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
        public IActionResult Add(AddRodFormModel rod)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == rod.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(rod.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(rod);
            }


            var currentRod = new Rod
            {
                Model = rod.Model,
                BrandId = brand.Id,
                CastingWeight = rod.CastingWeight,
                Image = rod.Image,
                PartsCount = rod.PartsCount,
                Price = rod.Price,
                Length = rod.Length,
                Weight = rod.Weight,
                Type = rod.Type,
                Description = rod.Description,
                Quantity = rod.Quantity
            };

            data.Rods.Add(currentRod);
            data.SaveChanges();

            return RedirectToAction("Add", "Rods");
        }
    }
}
