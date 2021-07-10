using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Baits;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class BaitsController:Controller
    {
        private ApplicationDbContext data;

        public BaitsController(ApplicationDbContext data)
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


    }
}
