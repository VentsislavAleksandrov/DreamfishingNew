using DreamFishingNew.Data;
using DreamFishingNew.Models.Clothes;
using DreamFishingNew.Models.Shared;
using DreamFishingNew.Services.Cloth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class ClothesController: Controller
    {
        private ApplicationDbContext data;
        private IClothService clothService;
        public ClothesController(ApplicationDbContext data, IClothService clothService)
        {
            this.data = data;
            this.clothService = clothService;
        }

        public IActionResult All([FromQuery]AllClothesQueryModel query)
        {
            var clothesQuery = clothService.GetAllClothes();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                clothesQuery = clothService.GetClothesByBrand(clothesQuery, query);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                clothesQuery = clothService.GetClothesBySerchrTerm(clothesQuery, query);
            }

            clothesQuery = clothService.GetClothesBySortTerm(clothesQuery, query);

            var clothes = clothService.GetClothesByPage(clothesQuery, query);

            var clothesBrands = clothService.GetClothesBrands();

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
            var brand = clothService.GetClothesBrandByName(clothes);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(clothes.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(clothes);
            }

            clothService.CreateClothes(clothes, brand);

            return RedirectToAction("Add", "Clothes");
        }

        public IActionResult Details(int id)
        {

            var clothes = clothService.GetClothesById(id);


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

        [Authorize]
        public IActionResult AddtoCart(int id, string userId)
        {
            //var currUser = data.Users.Where(x => x.Id == userId).FirstOrDefault();
            var currClothes = clothService.GetClothesById(id);

            currClothes.Quantity--;

            if (currClothes.Quantity < 0)
            {
                currClothes.Quantity = 0;
            }

            var bagModel = new AddtoCartViewModel
            {
                Model = currClothes.Model,
                Brand = currClothes.Brand.Name,
                Image = currClothes.Image, 
                Quantity = currClothes.Quantity
            };

            data.SaveChanges();
            return View(bagModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = clothService.GetClothesEditModel(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddClothesFormModel item)
        {
            var brand = clothService.GetClothesBrandByName(item);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(item);
            }

            clothService.EditClothes(id, item);

            return RedirectToAction("All", "Clothes");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {

            var clothes = clothService.GetClothesById(id);

            clothService.DeleteClothes(clothes);

            return RedirectToAction("All", "Clothes");
        }
    }
}
