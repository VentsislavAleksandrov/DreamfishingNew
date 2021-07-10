using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Clothes;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class ClothesController: Controller
    {
        private ApplicationDbContext data;

        public ClothesController(ApplicationDbContext data)
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
        public IActionResult Add(AddClothesFormModel clothes)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == clothes.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(clothes.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(clothes);
            }



            var currentClothes = new Clothes
            {
                Model = clothes.Model,
                BrandId = brand.Id,
                Image = clothes.Image,
                Price = clothes.Price,
                Weight = clothes.Weight,
                Size = clothes.Size,
                Waterproof = clothes.Waterproof.ToLower() == "yes",
                Material = clothes.Material,
                Description = clothes.Description,
                Quantity = clothes.Quantity
            };

            data.Clothes.Add(currentClothes);
            data.SaveChanges();

            return RedirectToAction("Add", "Clothes");
        }
    }
}
