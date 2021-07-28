using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Clothes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

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

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
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

        public IActionResult Details(int id)
        {

            var clothes = data
                .Clothes
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);


            var model = new ClothesDetailsViewModel
            {
                Id = clothes.Id,
                Model = clothes.Model,
                Brand = clothes.Brand.Name,
                Image = clothes.Image,
                Material = clothes.Material,
                Size = clothes.Size,
                Weight = clothes.Weight,
                Waterproof = clothes.Waterproof ? "Yes" : "No",
                Description = clothes.Description,
                Price = clothes.Price,
                Quantity = clothes.Quantity
            };

            return View(model);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = data.Clothes
                .Where(x => x.Id == id)
                .Select(x => new AddClothesFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                Material = x.Material,
                Size = x.Size,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Price = x.Price,
                Quantity = x.Quantity,
                Waterproof = x.Waterproof ? "yes" : "no"
                })
                .FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddClothesFormModel item)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == item.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(item);
            }


            var clothes = data
                .Clothes
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            clothes.Model = item.Model;
            clothes.Brand.Name = item.Brand;
            clothes.Size = item.Size;
            clothes.Description = item.Description;
            clothes.Image = item.Image;
            clothes.Waterproof = item.Waterproof.ToLower() == "yes" ? true : false;
            clothes.Price = item.Price;
            clothes.Quantity = item.Quantity;
            clothes.Weight = item.Weight;
            clothes.Material = item.Material;

            data.SaveChanges();

            return RedirectToAction("All", "Clothes");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var clothes = data
                .Clothes
                .Where(x => x.Id == id)
                .FirstOrDefault();

            data.Clothes.Remove(clothes);
            data.SaveChanges();

            return RedirectToAction("All", "Clothes");
        }
    }
}
