using DreamFishingNew.Models.Rods;
using DreamFishingNew.Models.Shared;
using DreamFishingNew.Services.Rods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class RodsController: Controller
    {
        
        private IRodService rodService;
        public RodsController( IRodService rodService)
        {
            
            this.rodService = rodService;
        }
        
        public IActionResult All([FromQuery] AllRodsQueryModel query)
        {

            var rodsQuery = rodService.GetAllRods();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                rodsQuery = rodService.GetRodsByBrand(rodsQuery, query);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                rodsQuery = rodService.GetRodsBySearchTerm(rodsQuery, query);
            }

            rodsQuery = rodService.GetRodsBySortTerm(rodsQuery, query);

            var rodsByPage = rodService.GetRodsByPage(rodsQuery, query);

            var rodBrands = rodService.GetRodBrands();


            var model = new AllRodsQueryModel
            {
                Brand = query.Brand,
                Brands = rodBrands,
                Rods = rodsByPage,
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
        public IActionResult Add(AddRodFormModel rod)
        {
            
            if (!ModelState.IsValid)
            {
                return View(rod);
            }

            var brand = rodService.GetRodBrandByName(rod);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(rod.Brand), "Brand does not exist.");
            }

            rodService.CreateRod(rod, brand);

            return RedirectToAction("Add", "Rods");
        }

        public IActionResult Details(int id)
        {

            var rod = rodService.GetRodById(id);


            var model = new RodDetailsViewModel
            {
                Id = rod.Id,
                Model = rod.Model,
                Brand = rod.Brand.Name,
                Image = rod.Image,
                CastingWeight = rod.CastingWeight,
                Length = rod.Length,
                PartsCount = rod.PartsCount,
                Weight = rod.Weight,
                Type = rod.Type,
                Description = rod.Description,
                Price = rod.Price,
                Quantity = rod.Quantity
            };

            return View(model);
        }

        [Authorize]
        public IActionResult AddtoCart(int id, string userId)
        {
            //var currUser = data.Users.Where(x => x.Id == userId).FirstOrDefault();
            var currRod = rodService.GetRodById(id);

            rodService.DecrementRodQuantity(currRod);

            var rodModel = new AddtoCartViewModel
            {
                Model = currRod.Model,
                Brand = currRod.Brand.Name,
                Image = currRod.Image, 
                Quantity = currRod.Quantity
            };
           
            return View(rodModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = rodService.GetRodEditModel(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddRodFormModel item)
        {           

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var brand = rodService.GetRodBrandByName(item);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }

            rodService.EditRod(id, item);

            return RedirectToAction("All", "Rods");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var rod = rodService.GetRodById(id);

            rodService.DeleteRod(rod);

            return RedirectToAction("All", "Rods");
        }
    }
}
