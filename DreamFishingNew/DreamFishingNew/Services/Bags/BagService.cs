using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Bags;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DreamFishingNew.Services.Bags
{
    public class BagService : IBagService
    {
        private ApplicationDbContext data;

        public BagService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void CreateBag(AddBagFormModel bag, Brand brand)
        {
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

           
        }

        public void DecrementBagQuantity(Bag currBag)
        {
            currBag.Quantity--;

            if (currBag.Quantity < 0)
            {
                currBag.Quantity = 0;
            }

            data.SaveChanges();
        }

        public void DeleteBag(Bag bag)
        {
            data.Bags.Remove(bag);
            data.SaveChanges();
        }

        public void Editbag(Bag bag, AddBagFormModel item)
        {
            bag.Model = item.Model;
            bag.Brand.Name = item.Brand;
            bag.Description = item.Description;
            bag.Image = item.Image;
            bag.Size = item.Size;
            bag.Price = item.Price;
            bag.Quantity = item.Quantity;
            bag.Weight = item.Weight;

            data.SaveChanges();
        }

        public ICollection<Bag> GetAllBags()
        {
            var bagsQuery = data.Bags
                .Include("Brand")
                .ToList();

            return bagsQuery;
        }

        public Brand GetBagBrand(AddBagFormModel bag)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == bag.Brand.ToLower());

            return brand;
        }

        public ICollection<string> GetBagBrands()
        {
            var bagBrands = data
                .Bags
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();

            return bagBrands;
        }

        public Bag GetBagById(int id)
        {
            var bag = data
                .Bags
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            return bag;
        }

        public AddBagFormModel GetBagEditFormModel(int id)
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

            return model;
        }

        public ICollection<Bag> GetBagsByBrand(AllBagsQueryModel query, ICollection<Bag> bagsQuery)
        {
              bagsQuery = bagsQuery
                    .Where(x => x.Brand.Name.ToLower() == query.Brand.ToLower())
                    .ToList();

            return bagsQuery;
        }

        public ICollection<BagListingViewModel> GetBagsByPage(AllBagsQueryModel query, ICollection<Bag> bagsQuery)
        {
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

            return bags;
        }

        public ICollection<Bag> GetBagsBySearchTerm(string searchterm, ICollection<Bag> bagsQuery)
        {
             bagsQuery = bagsQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(searchterm.ToLower())
                    ||x.Description.ToLower().Contains(searchterm.ToLower())
                    )
                    .ToList();

            return bagsQuery;
        }

        public ICollection<Bag> SortBagsBySortTerm(BagSorting sortTerm, ICollection<Bag> bagsQuery)
        {
            bagsQuery = sortTerm switch
            {
                BagSorting.MinPrice => bagsQuery.OrderBy(x => x.Price).ToList(),
                BagSorting.MaxPrice => bagsQuery.OrderByDescending(x => x.Price).ToList(),
                BagSorting.BrandAndModel => bagsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => bagsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            return bagsQuery;
        }
    }
}
