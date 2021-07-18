using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Clothes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult All([FromQuery]AllClothesQueryModel query)
        {
            var clothesQuery = data.Clothes
                .Include<Clothes>("Brand")
                .ToList();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                clothesQuery = data.Clothes
                    .Where(x => x.Brand.Name.ToLower() == query.Brand)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                clothesQuery = clothesQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();
            }

            clothesQuery = query.Sorting switch
            {
                ClothesSorting.MinPrice => clothesQuery.OrderBy(x => x.Price).ToList(),
                ClothesSorting.MaxPrice => clothesQuery.OrderByDescending(x => x.Price).ToList(),
                ClothesSorting.BrandAndModel => clothesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => clothesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            var clothes = clothesQuery
                .Skip((query.currentPage -1) * AllClothesQueryModel.ClothesPerPage)
                .Take(AllClothesQueryModel.ClothesPerPage)
                .Select(x => new ClothesListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            


            var clothesBrands = data
                .Clothes
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();


            var model = new AllClothesQueryModel
            {
                Brand = query.Brand,
                Brands = clothesBrands,
                Clothes = clothes,
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
