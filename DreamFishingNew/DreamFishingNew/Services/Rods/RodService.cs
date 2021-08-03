using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Rods;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DreamFishingNew.Services.Rods
{
    public class RodService: IRodService
    {
        private ApplicationDbContext data;

        public RodService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void CreateRod(AddRodFormModel rod, Brand brand)
        {
            var currentRod = new Rod
            {
                Model = rod.Model,
                BrandId = brand.Id,
                CastingWeight = rod.CastingWeight,
                Image = rod.Image,
                PartsCount = rod.PartsCount,
                Price = rod.Price,
                Length = rod.Length,
                Weight = rod.Weight,
                Type = rod.Type,
                Description = rod.Description,
                Quantity = rod.Quantity
            };

            data.Rods.Add(currentRod);
            data.SaveChanges();
        }

        public void DecrementRodQuantity(Rod currRod)
        {
            currRod.Quantity--;

            if (currRod.Quantity < 0)
            {
                currRod.Quantity = 0;
            } 

            data.SaveChanges();
        }

        public void DeleteRod(Rod rod)
        {
            data.Rods.Remove(rod);
            data.SaveChanges();
        }

        public void EditRod(int id, AddRodFormModel item)
        {
            var rod = data
                .Rods
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            rod.Model = item.Model;
            rod.Brand.Name = item.Brand;
            rod.CastingWeight = item.CastingWeight;
            rod.Description = item.Description;
            rod.Image = item.Image;
            rod.Length = item.Length;
            rod.PartsCount = item.PartsCount;
            rod.Price = item.Price;
            rod.Quantity = item.Quantity;
            rod.Type = item.Type;
            rod.Weight = item.Weight;

            data.SaveChanges();
        }

        public ICollection<Rod> GetAllRods()
        {
           var rodsQuery = data.Rods
                .Include("Brand")
                .ToList();

            return rodsQuery;
        }

        public Brand GetRodBrandByName(AddRodFormModel rod)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == rod.Brand.ToLower());

            return brand;
        }

        public ICollection<string> GetRodBrands()
        {
            var rodBrands = data
                .Rods
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();

            return rodBrands;
        }

        public Rod GetRodById(int id)
        {
            var rod = data
                .Rods
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            return rod;
        }

        public AddRodFormModel GetRodEditModel(int id)
        {
            var model = data.Rods
                .Where(x => x.Id == id)
                .Select(x => new AddRodFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                Length = x.Length,
                PartsCount = x.PartsCount,
                CastingWeight = x.CastingWeight,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Type = x.Type,
                Price = x.Price,
                Quantity = x.Quantity
                })
                .FirstOrDefault();

            return model;
        }

        public ICollection<Rod> GetRodsByBrand(ICollection<Rod> rodsQuery, AllRodsQueryModel query)
        {
            rodsQuery = rodsQuery
                    .Where(x => x.Brand.Name.ToLower() == query.Brand.ToLower())
                    .ToList();

            return rodsQuery;
        }

        public ICollection<RodListingViewModel> GetRodsByPage(ICollection<Rod> rodsQuery, AllRodsQueryModel query)
        {
            var rods = rodsQuery
                .Skip((query.CurrentPage -1) * AllRodsQueryModel.RodsPerPage)
                .Take(AllRodsQueryModel.RodsPerPage)
                .Select(x => new RodListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                    Type = x.Type
                })
                .ToList();

            return rods;
        }

        public ICollection<Rod> GetRodsBySearchTerm(ICollection<Rod> rodsQuery, AllRodsQueryModel query)
        {
            rodsQuery = rodsQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    || x.Type.ToLower().Contains(query.SearchTerm.ToLower())
                    || x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();

            return rodsQuery;
        }

        public ICollection<Rod> GetRodsBySortTerm(ICollection<Rod> rodsQuery, AllRodsQueryModel query)
        {
            rodsQuery = query.Sorting switch
            {
                RodSorting.MinPrice => rodsQuery.OrderBy(x => x.Price).ToList(),
                RodSorting.MaxPrice => rodsQuery.OrderByDescending(x => x.Price).ToList(),
                RodSorting.lenght => rodsQuery.OrderByDescending(x => x.Length).ToList(),
                RodSorting.BrandAndModel => rodsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => rodsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            return rodsQuery;
        }
    }
}
