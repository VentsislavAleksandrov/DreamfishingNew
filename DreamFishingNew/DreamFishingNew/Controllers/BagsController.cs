using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Infrastructure;
using DreamFishingNew.Models.Bags;
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

        public BagsController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery]AllBagsQueryModel query)
        {
            var bagsQuery = data.Bags
                .Include<Bag>("Brand")
                .ToList();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                bagsQuery = data.Bags
                    .Where(x => x.Brand.Name.ToLower() == query.Brand)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                bagsQuery = bagsQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();
            }

            bagsQuery = query.Sorting switch
            {
                BagSorting.MinPrice => bagsQuery.OrderBy(x => x.Price).ToList(),
                BagSorting.MaxPrice => bagsQuery.OrderByDescending(x => x.Price).ToList(),
                BagSorting.BrandAndModel => bagsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => bagsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            var bags = bagsQuery
                .Skip((query.currentPage -1) * AllBagsQueryModel.BagsPerPage)
                .Take(AllBagsQueryModel.BagsPerPage)
                .Select(x => new BagListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            


            var bagBrands = data
                .Baits
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();


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
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == bag.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(bag.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(bag);
            }


            var currentBag = new Bag
            {
                Model = bag.Model,
                BrandId = brand.Id,
                Image = bag.Image,
                Price = bag.Price,
                Weight = bag.Weight,
                Size = bag.Size,
                Description = bag.Description,
                Quantity = bag.Quantity
            };

            data.Bags.Add(currentBag);
            data.SaveChanges();

            return RedirectToAction("Add", "Bags");
        }

        public IActionResult Details(int id)
        {
            var UserId = this.User.Id();

            var currUser = data.Users.Where(x => x.Id == UserId);

            var pk = currUser.Select(x => x.ProductCart).FirstOrDefault();

            var bag = data
                .Bags
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);


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
            var currUser = data.Users.Where(x => x.Id == userId).FirstOrDefault();
            var currBag = data
                .Bags
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

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
            var model = data.Bags
                .Where(x => x.Id == id)
                .Select(x => new AddBagFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                Size = x.Size,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Price = x.Price,
                Quantity = x.Quantity
                })
                .FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddBagFormModel item)
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


            var bag = data
                .Bags
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            bag.Model = item.Model;
            bag.Brand.Name = item.Brand;
            bag.Description = item.Description;
            bag.Image = item.Image;
            bag.Size = item.Size;
            bag.Price = item.Price;
            bag.Quantity = item.Quantity;
            bag.Weight = item.Weight;

            data.SaveChanges();

            return RedirectToAction("All", "Bags");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var bag = data
                .Bags
                .Where(x => x.Id == id)
                .FirstOrDefault();

            data.Bags.Remove(bag);
            data.SaveChanges();

            return RedirectToAction("All", "Bags");
        }
    }
}
