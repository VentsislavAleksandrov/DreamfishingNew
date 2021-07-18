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
    }
}
