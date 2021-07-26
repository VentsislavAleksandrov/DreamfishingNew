using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Reels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class ReelsController: Controller
    {
        private ApplicationDbContext data;

        public ReelsController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery]AllReelsQueryModel query)
        {
            var reelsQuery = data.Reels
                .Include<Reel>("Brand")
                .ToList();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                reelsQuery = data.Reels
                    .Where(x => x.Brand.Name.ToLower() == query.Brand)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                reelsQuery = reelsQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();
            }

            reelsQuery = query.Sorting switch
            {
                ReelSorting.MinPrice => reelsQuery.OrderBy(x => x.Price).ToList(),
                ReelSorting.MaxPrice => reelsQuery.OrderByDescending(x => x.Price).ToList(),
                ReelSorting.BrandAndModel => reelsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => reelsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            var reels = reelsQuery
                .Skip((query.currentPage -1) * AllReelsQueryModel.ReelsPerPage)
                .Take(AllReelsQueryModel.ReelsPerPage)
                .Select(x => new ReelListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            


            var reelBrands = data
                .Reels
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();


            var model = new AllReelsQueryModel
            {
                Brand = query.Brand,
                Brands = reelBrands,
                Reels = reels,
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
        public IActionResult Add(AddReelFormModel reel)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == reel.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(reel.Brand),"Brand does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(reel);
            }

            var currentReel = new Reel
            {
                Model = reel.Model,
                BrandId = brand.Id,
                Description = reel.Description,
                Price = reel.Price,
                GearRatio = reel.GearRatio,
                Quantity = reel.Quantity,
                Image = reel.Image,
                LineCapacity = reel.LineCapacity,
                Weight = reel.Weight,
                BalbearingsCount = reel.BalbearingsCount
                
            };

            data.Reels.Add(currentReel);
            data.SaveChanges();

            return RedirectToAction("Add","Reels");
        }

        public IActionResult Details(int id)
        {

            var reel = data
                .Reels
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);


            var model = new ReelDetailsViewModel
            {
                Model = reel.Model,
                Brand = reel.Brand.Name,
                Image = reel.Image,
                BalbearingsCount = reel.BalbearingsCount,
                GearRatio = reel.GearRatio,
                LineCapacity = reel.LineCapacity,
                Weight = reel.Weight,
                Description = reel.Description,
                Price = reel.Price,
                Quantity = reel.Quantity
            };

            return View(model);
        }

         public IActionResult Edit(int id)
        {
            var model = data.Reels
                .Where(x => x.Id == id)
                .Select(x => new AddReelFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                GearRatio = x.GearRatio,
                BalbearingsCount = x.BalbearingsCount,
                LineCapacity = x.LineCapacity,
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
        public IActionResult Edit(int id, AddReelFormModel item)
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


            var reel = data
                .Reels
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            reel.Model = item.Model;
            reel.Brand.Name = item.Brand;
            reel.LineCapacity = item.LineCapacity;
            reel.Description = item.Description;
            reel.Image = item.Image;
            reel.GearRatio = item.GearRatio;
            reel.BalbearingsCount = item.BalbearingsCount;
            reel.Price = item.Price;
            reel.Quantity = item.Quantity;
            reel.Weight = item.Weight;

            data.SaveChanges();

            return RedirectToAction("All", "Reels");
        }

        public IActionResult Delete(int id)
        {
            var reel = data
                .Reels
                .Where(x => x.Id == id)
                .FirstOrDefault();

            data.Reels.Remove(reel);
            data.SaveChanges();

            return RedirectToAction("All", "Rods");
        }
    }
}
