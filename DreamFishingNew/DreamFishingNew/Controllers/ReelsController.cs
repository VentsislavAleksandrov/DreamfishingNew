using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Reels;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult All()
        {
            return View();
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
