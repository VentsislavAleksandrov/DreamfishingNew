using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Rods;
using DreamFishingNew.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class RodsController: Controller
    {
        private ApplicationDbContext data;

        public RodsController(ApplicationDbContext data)
        {
            this.data = data;
        }
        //Cannot convert type System.Security.Claims.ClaimsPrincipal to DreamfishingNew.Data.Models.ApplicationUser
        public IActionResult All([FromQuery] AllRodsQueryModel query)
        {
            
            var rodsQuery = data.Rods
                .Include("Brand")
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

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
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

        public IActionResult Details(int id)
        {
            
            var rod = data
                .Rods
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);


            var model = new RodDetailsViewModel
            {
                Id = rod.Id,
                Model = rod.Model,
                Brand = rod.Brand.Name,
                Image = rod.Image,
                CastingWeight = rod.CastingWeight,
                Length = rod.Length,
                PartsCount = rod.PartsCount,
                Weight = rod.Weight,
                Type = rod.Type,
                Description = rod.Description,
                Price = rod.Price,
                Quantity = rod.Quantity
            };

            return View(model);
        }

        [Authorize]
        public IActionResult AddtoCart(int id, string userId)
        {
            //var currUser = data.Users.Where(x => x.Id == userId).FirstOrDefault();
            var currRod = data
                .Rods
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            currRod.Quantity--;

            if (currRod.Quantity < 0)
            {
                currRod.Quantity = 0;
            }

            var bagModel = new AddtoCartViewModel
            {
                Model = currRod.Model,
                Brand = currRod.Brand.Name,
                Image = currRod.Image, 
                Quantity = currRod.Quantity
            };

            data.SaveChanges();
            return View(bagModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = data.Rods
                .Where(x => x.Id == id)
                .Select(x => new AddRodFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                Length = x.Length,
                PartsCount = x.PartsCount,
                CastingWeight = x.CastingWeight,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Type = x.Type,
                Price = x.Price,
                Quantity = x.Quantity
                })
                .FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddRodFormModel item)
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


            var rod = data
                .Rods
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            rod.Model = item.Model;
            rod.Brand.Name = item.Brand;
            rod.CastingWeight = item.CastingWeight;
            rod.Description = item.Description;
            rod.Image = item.Image;
            rod.Length = item.Length;
            rod.PartsCount = item.PartsCount;
            rod.Price = item.Price;
            rod.Quantity = item.Quantity;
            rod.Type = item.Type;
            rod.Weight = item.Weight;

            data.SaveChanges();

            return RedirectToAction("All", "Rods");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var rod = data
                .Rods
                .Where(x => x.Id == id)
                .FirstOrDefault();

            data.Rods.Remove(rod);
            data.SaveChanges();

            return RedirectToAction("All", "Rods");
        }
    }
}
