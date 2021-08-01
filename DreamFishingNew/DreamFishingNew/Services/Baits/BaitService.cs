using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Baits;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DreamFishingNew.Services.Baits
{
    public class BaitService: IBaitService
    {
        private ApplicationDbContext data;

        public BaitService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void CreateBait(AddBaitFormModel bait, Brand brand)
        {
            var currentBait = new Bait
            {
                Model = bait.Model,
                BrandId = brand.Id,
                Image = bait.Image,
                Price = bait.Price,
                Length = bait.Length,
                Weight = bait.Weight,
                Type = bait.Type,
                Description = bait.Description,
                Quantity = bait.Quantity
            };

            data.Baits.Add(currentBait);
            data.SaveChanges();
        }

        public void DeleteBait(Bait bait)
        {
            data.Baits.Remove(bait);
            data.SaveChanges();
        }

        public void EditBait(int id, AddBaitFormModel item)
        {
            var bait = data
                .Baits
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            bait.Model = item.Model;
            bait.Brand.Name = item.Brand;
            bait.Length = item.Length;
            bait.Description = item.Description;
            bait.Image = item.Image;
            bait.Type = item.Type;
            bait.Price = item.Price;
            bait.Quantity = item.Quantity;
            bait.Weight = item.Weight;

            data.SaveChanges();
        }

        public ICollection<Bait> GetAllBaits()
        {
            var baitsQuery = data.Baits
                .Include("Brand")
                .ToList();
            return baitsQuery;
        }

        public Brand GetBaitBrand(AddBaitFormModel bait)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == bait.Brand.ToLower());

            return brand;
        }

        public Brand GetBaitBrandByName(AddBaitFormModel bait)
        {
           var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == bait.Brand.ToLower());

            return brand;
        }

        public Bait GetBaitById(int id )
        {
            var bait = data
                .Baits
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            return bait;
        }

        public AddBaitFormModel GetBaitEditModel(int id)
        {
            var model = data.Baits
                .Where(x => x.Id == id)
                .Select(x => new AddBaitFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                Length = x.Length,
                Type = x.Type,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Price = x.Price,
                Quantity = x.Quantity
                })
                .FirstOrDefault();

            return model;
        }

        public ICollection<Bait> GetBaitsByBrand(ICollection<Bait> baits, AllBaitsQueryModel query)
        {
            var baitsQuery = baits
                    .Where(x => x.Brand.Name.ToLower() == query.Brand.ToLower())
                    .ToList();

            return baitsQuery;
        }

        public ICollection<BaitListingViewModel> GetBaitsByPage(ICollection<Bait> baitsQuery, AllBaitsQueryModel query)
        {
            var baits = baitsQuery
                .Skip((query.currentPage -1) * AllBaitsQueryModel.BaitsPerPage)
                .Take(AllBaitsQueryModel.BaitsPerPage)
                .Select(x => new BaitListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            return baits;
        }

        public ICollection<Bait> GetBaitsBySearchTerm(ICollection<Bait> baitsQuery, AllBaitsQueryModel query)
        {
            baitsQuery = baitsQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();

            return baitsQuery;
        }

        public ICollection<Bait> GetBaitsBySortTerm(BaitSorting sorting, ICollection<Bait> baitsQuery)
        {
            baitsQuery = sorting switch
            {
                BaitSorting.MinPrice => baitsQuery.OrderBy(x => x.Price).ToList(),
                BaitSorting.MaxPrice => baitsQuery.OrderByDescending(x => x.Price).ToList(),
                BaitSorting.BrandAndModel => baitsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => baitsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            return baitsQuery;
        }

        public ICollection<string> GetBiteBrands()
        {
            var baitBrands = data
                .Baits
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();

            return baitBrands;
        }
    }
}
