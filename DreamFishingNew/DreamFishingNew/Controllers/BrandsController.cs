using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Brands;
using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    public class BrandsController: Controller
    {
        private ApplicationDbContext data;

        public BrandsController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddBrandFormModel brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            var currentBrand = new Brand
            {
                Name = brand.Name,
                Country = brand.Country
            };

            data.Brands.Add(currentBrand);
            data.SaveChanges();

            return RedirectToAction("Add", "Brands");
        }
    }
}
