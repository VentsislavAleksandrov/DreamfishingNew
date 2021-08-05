using DreamFishingNew.Models.Bags;
using DreamFishingNew.Models.Shared;
using DreamFishingNew.Services.Bags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class BagsController: Controller
    {
        
        private IBagService bagService;


        public BagsController( IBagService bagService)
        {
            
            this.bagService = bagService;

        }

        public IActionResult All([FromQuery]AllBagsQueryModel query)
        {
            var bagsQuery = this.bagService.GetAllBags();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                bagsQuery = bagService.GetBagsByBrand(query, bagsQuery);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                bagsQuery = bagService.GetBagsBySearchTerm(query.SearchTerm, bagsQuery);
            }

            bagsQuery = bagService.SortBagsBySortTerm(query.Sorting, bagsQuery);

            var bagsByPage = bagService.GetBagsByPage(query, bagsQuery);


            var bagBrands = bagService.GetBagBrands();


            var model = new AllBagsQueryModel
            {
                Brand = query.Brand,
                Brands = bagBrands,
                Bags = bagsByPage,
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
 
            if (!ModelState.IsValid)
            {
                return View(bag);
            }

            var brand = bagService.GetBagBrand(bag);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(bag.Brand), "Brand does not exist.");
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

            bagService.DecrementBagQuantity(currBag);

            var bagModel = new AddtoCartViewModel
            {
                Model = currBag.Model,
                Brand = currBag.Brand.Name,
                Image = currBag.Image, 
                Quantity = currBag.Quantity
            };
           
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
            
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var brand = bagService.GetBagBrand(item);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
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
