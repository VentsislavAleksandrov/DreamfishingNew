using DreamFishingNew.Data;
using DreamFishingNew.Models.Baits;
using DreamFishingNew.Models.Shared;
using DreamFishingNew.Services.Baits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class BaitsController:Controller
    {
        private ApplicationDbContext data;
        private IBaitService baitService;
        public BaitsController(ApplicationDbContext data, IBaitService baitService)
        {
            this.data = data;
            this.baitService = baitService;
        }

        public IActionResult All([FromQuery]AllBaitsQueryModel query)
        {
            var baitsQuery = baitService.GetAllBaits();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                baitsQuery = baitService.GetBaitsByBrand(baitsQuery, query);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                baitsQuery = baitService.GetBaitsBySearchTerm(baitsQuery, query);
            }

            baitsQuery = baitService.GetBaitsBySortTerm(query.Sorting, baitsQuery);

            var baits = baitService.GetBaitsByPage(baitsQuery, query);

            var baitBrands = baitService.GetBiteBrands();


            var model = new AllBaitsQueryModel
            {
                Brand = query.Brand,
                Brands = baitBrands,
                Baits = baits,
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
        public IActionResult Add(AddBaitFormModel bait)
        {

            if (!ModelState.IsValid)
            {
                return View(bait);
            }

            var brand = baitService.GetBaitBrandByName(bait);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(bait.Brand), "Brand does not exist.");
            }

            baitService.CreateBait(bait, brand);

            return RedirectToAction("Add", "Baits");
        }

        public IActionResult Details(int id)
        {

            var bait = baitService.GetBaitById(id);


            var model = new BaitDetailsViewModel
            {
                Id = bait.Id,
                Model = bait.Model,
                Brand = bait.Brand.Name,
                Image = bait.Image,
                Length = bait.Length,
                Type = bait.Type,
                Weight = bait.Weight,
                Description = bait.Description,
                Price = bait.Price,
                Quantity = bait.Quantity
            };

            return View(model);
        }

        [Authorize]
        public IActionResult AddtoCart(int id, string userId)
        {
            //var currUser = data.Users.Where(x => x.Id == userId).FirstOrDefault();
            var currBait = baitService.GetBaitById(id);

            baitService.DecrementBaitQuantity(currBait);

            var baitModel = new AddtoCartViewModel
            {
                Model = currBait.Model,
                Brand = currBait.Brand.Name,
                Image = currBait.Image, 
                Quantity = currBait.Quantity
            };
            
            return View(baitModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = baitService.GetBaitEditModel(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddBaitFormModel item)
        {
            
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var brand = baitService.GetBaitBrandByName(item);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }

            baitService.EditBait(id, item);

            return RedirectToAction("All", "Baits");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var bait = baitService.GetBaitById(id);

            baitService.DeleteBait(bait);
   
            return RedirectToAction("All", "Baits");
        }
    }
}
