using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Glasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult All([FromQuery]AllGlassesQueryModel query)
        {
            var glassesQuery = data.Glasses
                .Include<Glasses>("Brand")
                .ToList();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                glassesQuery = data.Glasses
                    .Where(x => x.Brand.Name.ToLower() == query.Brand)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                glassesQuery = glassesQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();
            }

            glassesQuery = query.Sorting switch
            {
                GlassesSorting.MinPrice => glassesQuery.OrderBy(x => x.Price).ToList(),
                GlassesSorting.MaxPrice => glassesQuery.OrderByDescending(x => x.Price).ToList(),
                GlassesSorting.BrandAndModel => glassesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => glassesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            var glasses = glassesQuery
                .Skip((query.currentPage -1) * AllGlassesQueryModel.GlassesPerPage)
                .Take(AllGlassesQueryModel.GlassesPerPage)
                .Select(x => new GlassesListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            


            var glassesBrands = data
                .Glasses
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();


            var model = new AllGlassesQueryModel
            {
                Brand = query.Brand,
                Brands = glassesBrands,
                Glasses = glasses,
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

        public IActionResult Details(int id)
        {

            var glasses = data
                .Glasses
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);


            var model = new GlassesDetailsViewModel
            {
                Model = glasses.Model,
                Brand = glasses.Brand.Name,
                Image = glasses.Image,
                Weight = glasses.Weight,
                Description = glasses.Description,
                Price = glasses.Price,
                Quantity = glasses.Quantity
            };

            return View(model);
        }
    }
}
