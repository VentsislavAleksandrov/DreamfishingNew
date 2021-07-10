using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Bags;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class BagsController: Controller
    {
        private ApplicationDbContext data;

        public BagsController(ApplicationDbContext data)
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
        public IActionResult Add(AddBagFormModel bag)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == bag.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(bag.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(bag);
            }


            var currentBag = new Bag
            {
                Model = bag.Model,
                BrandId = brand.Id,
                Image = bag.Image,
                Price = bag.Price,
                Weight = bag.Weight,
                Size = bag.Size,
                Description = bag.Description,
                Quantity = bag.Quantity
            };

            data.Bags.Add(currentBag);
            data.SaveChanges();

            return RedirectToAction("Add", "Bags");
        }
    }
}
