using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Lines;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class LinesController: Controller
    {
        private ApplicationDbContext data;

        public LinesController(ApplicationDbContext data)
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
        public IActionResult Add(AddLineFormModel line)
        {
             var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == line.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(line.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(line);
            }


            var currentLine = new Line
            {
                Model = line.Model,
                BrandId = brand.Id,
                Image = line.Image,
                Price = line.Price,
                Length = line.Length,
                Weight = line.Weight,
                Size = line.Size,
                Description = line.Description,
                Quantity = line.Quantity
            };

            data.Lines.Add(currentLine);
            data.SaveChanges();

            return RedirectToAction("Add", "Rods");
        }
    }
}
