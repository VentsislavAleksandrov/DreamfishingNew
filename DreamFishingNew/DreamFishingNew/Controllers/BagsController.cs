using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Bags;
using DreamFishingNew.Models.Shared;
using DreamFishingNew.Services.Bags;
using DreamFishingNew.Services.Brands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class BagsController: Controller
    {
        private ApplicationDbContext data;
        private IBagService bagService;
        private IBrandService brandService;

        public BagsController(ApplicationDbContext data, IBagService bagService, IBrandService brandService)
        {
            this.data = data;
            this.bagService = bagService;
            this.brandService = brandService;
        }

        public IActionResult All([FromQuery]AllBagsQueryModel query)
        {
            var bagsQuery = this.bagService.GetAllBags();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                bagsQuery = bagService.GetBagsByBrand(query.Brand, bagsQuery);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                bagsQuery = bagService.GetBagsBySearchTerm(query.SearchTerm, bagsQuery);
            }

            bagsQuery = bagService.SortBagsBySortTerm(query.Sorting, bagsQuery);

            var bags = bagService.GetBagsByPage(query, bagsQuery);


            var bagBrands = bagService.GetBagBrands();


            var model = new AllBagsQueryModel
            {
                Brand = query.Brand,
                Brands = bagBrands,
                Bags = bags,
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
        public IActionResult Add(AddBagFormModel bag)
        {
            var brand = brandService.GetBrand(bag);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(bag.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(bag);
            }


            bagService.CreateBag(bag, brand);

            

            return RedirectToAction("Add", "Bags");
        }

        public IActionResult Details(int id)
        {


            var bag = bagService.GetBagById(id);


            var model = new BagDetailsViewModel
            {
                Id = bag.Id,
                Model = bag.Model,
                Brand = bag.Brand.Name,
                Image = bag.Image,
                Weight = bag.Weight,
                Description = bag.Description,
                Size = bag.Size,
                Price = bag.Price,
                Quantity = bag.Quantity
            };

            return View(model);
        }

        [Authorize]
        public IActionResult AddtoCart(int id, string userId)
        {
            //var currUser = data.Users.Where(x => x.Id == userId).FirstOrDefault();
            var currBag = bagService.GetBagById(id);

            currBag.Quantity--;

            if (currBag.Quantity < 0)
            {
                currBag.Quantity = 0;
            }

            var bagModel = new AddtoCartViewModel
            {
                Model = currBag.Model,
                Brand = currBag.Brand.Name,
                Image = currBag.Image, 
                Quantity = currBag.Quantity
            };

            data.SaveChanges();
            return View(bagModel);
        }


        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = bagService.GetBagEditFormModel(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddBagFormModel item)
        {
            var brand = brandService.GetBrandByFormModel(item);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(item);
            }


            var bag = bagService.GetBagById(id);

            bagService.Editbag(bag, item);
            

            return RedirectToAction("All", "Bags");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var bag = bagService.GetBagById(id);

            bagService.DeleteBag(bag);

            return RedirectToAction("All", "Bags");
        }
    }
}
