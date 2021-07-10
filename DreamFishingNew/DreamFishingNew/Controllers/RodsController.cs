using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Rods;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult All ()
        {
            return View();
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
