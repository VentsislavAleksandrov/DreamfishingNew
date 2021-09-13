using DreamFishingNew.Models.Reels;
using DreamFishingNew.Models.Shared;
using DreamFishingNew.Services.Reels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class ReelsController : Controller
    {
        private IReelService reelService;

        public ReelsController(IReelService reelService)
        {
            this.reelService = reelService;
        }

        public IActionResult All([FromQuery] AllReelsQueryModel query)
        {
            var reelsQuery = reelService.GetAllReels();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                reelsQuery = reelService.GetReelsByBrand(reelsQuery, query);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                reelsQuery = reelService.GetReelsBySearchTerm(reelsQuery, query);
            }

            reelsQuery = reelService.GetReelsBySortTerm(reelsQuery, query);

            var reelsByPage = reelService.GetReelsByPage(reelsQuery, query);

            var reelBrands = reelService.GetReelBrands();

            var model = new AllReelsQueryModel
            {
                Brand = query.Brand,
                Brands = reelBrands,
                Reels = reelsByPage,
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
        public IActionResult Add(AddReelFormModel reel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(reel);
            }

            var brand = reelService.GetReelBrandByName(reel);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(reel.Brand), "Brand does not exist.");
            }

            reelService.CreateReel(reel, brand);

            return RedirectToAction("Add", "Reels");
        }

        public IActionResult Details(int id)
        {
            var reel = reelService.GetReelById(id);

            var model = new ReelDetailsViewModel
            {
                Id = reel.Id,
                Model = reel.Model,
                Brand = reel.Brand.Name,
                Image = reel.Image,
                BalbearingsCount = reel.BalbearingsCount,
                GearRatio = reel.GearRatio,
                LineCapacity = reel.LineCapacity,
                Weight = reel.Weight,
                Description = reel.Description,
                Price = reel.Price,
                Quantity = reel.Quantity
            };

            return View(model);
        }

        [Authorize]
        public IActionResult AddtoCart(int id)
        {
            var currReel = reelService.GetReelById(id);

            reelService.DecrementReelQuantity(currReel);

            var reelModel = new AddtoCartViewModel
            {
                Model = currReel.Model,
                Brand = currReel.Brand.Name,
                Image = currReel.Image,
                Quantity = currReel.Quantity
            };

            return View(reelModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = reelService.GetReelEditModel(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddReelFormModel item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var brand = reelService.GetReelBrandByName(item);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }

            reelService.EditReel(id, item);

            return RedirectToAction("All", "Reels");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var reel = reelService.GetReelById(id);

            reelService.DeleteReel(reel);

            return RedirectToAction("All", "Reels");
        }
    }
}
