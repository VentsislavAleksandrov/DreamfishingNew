using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Glasses;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class GlassesController: Controller
    {
        private ApplicationDbContext data;

        public GlassesController(ApplicationDbContext data)
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
        public IActionResult Add(AddGlassesFormModel glasses)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == glasses.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(glasses.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(glasses);
            }


            var currentGlasses = new Glasses
            {
                Model = glasses.Model,
                BrandId = brand.Id,
                Image = glasses.Image,
                Price = glasses.Price,
                Weight = glasses.Weight,
                Description = glasses.Description,
                Quantity = glasses.Quantity
            };

            data.Glasses.Add(currentGlasses);
            data.SaveChanges();

            return RedirectToAction("Add", "Glasses");
        }
    }
}
