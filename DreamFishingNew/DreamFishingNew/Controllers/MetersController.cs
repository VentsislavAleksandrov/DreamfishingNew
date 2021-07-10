using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Meters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class MetersController: Controller
    {
        private ApplicationDbContext data;

        public MetersController(ApplicationDbContext data)
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
        public IActionResult Add(AddMeterFormModel meter)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == meter.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(meter.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(meter);
            }


            var currentMeter = new Meter
            {
                Model = meter.Model,
                BrandId = brand.Id,
                Image = meter.Image,
                Price = meter.Price,
                Weight = meter.Weight,
                Length = meter.Length,
                Description = meter.Description,
                Quantity = meter.Quantity
            };

            data.Meters.Add(currentMeter);
            data.SaveChanges();

            return RedirectToAction("Add", "Meters");
        }
    }
}
