using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Reels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DreamFishingNew.Services.Reels
{
    public class ReelsService: IReelService
    {
        private ApplicationDbContext data;

        public ReelsService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void CreateReel(AddReelFormModel reel, Brand brand)
        {
            var currentReel = new Reel
            {
                Model = reel.Model,
                BrandId = brand.Id,
                Description = reel.Description,
                Price = reel.Price,
                GearRatio = reel.GearRatio,
                Quantity = reel.Quantity,
                Image = reel.Image,
                LineCapacity = reel.LineCapacity,
                Weight = reel.Weight,
                BalbearingsCount = reel.BalbearingsCount
                
            };

            data.Reels.Add(currentReel);
            data.SaveChanges();
        }

        public void DeleteReel(Reel reel)
        {
            data.Reels.Remove(reel);
            data.SaveChanges();
        }

        public void EditReel(int id, AddReelFormModel item)
        {
            var reel = data
                .Reels
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            reel.Model = item.Model;
            reel.Brand.Name = item.Brand;
            reel.LineCapacity = item.LineCapacity;
            reel.Description = item.Description;
            reel.Image = item.Image;
            reel.GearRatio = item.GearRatio;
            reel.BalbearingsCount = item.BalbearingsCount;
            reel.Price = item.Price;
            reel.Quantity = item.Quantity;
            reel.Weight = item.Weight;

            data.SaveChanges();
        }

        public ICollection<Reel> GetAllReels()
        {
            var reelsQuery = data.Reels
                .Include("Brand")
                .ToList();

            return reelsQuery;
        }

        public Brand GetReelBrandByName(AddReelFormModel reel)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == reel.Brand.ToLower());

            return brand;
        }

        public ICollection<string> GetReelBrands()
        {
            var reelBrands = data
                .Reels
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();

            return reelBrands;
        }

        public Reel GetReelById(int id)
        {
            var reel = data
                .Reels
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            return reel;
        }

        public AddReelFormModel GetReelEditModel(int id)
        {
           var model = data.Reels
                .Where(x => x.Id == id)
                .Select(x => new AddReelFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                GearRatio = x.GearRatio,
                BalbearingsCount = x.BalbearingsCount,
                LineCapacity = x.LineCapacity,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Price = x.Price,
                Quantity = x.Quantity
                })
                .FirstOrDefault();

            return model;
        }

        public ICollection<Reel> GetReelsByBrand(ICollection<Reel> reelsQuery, AllReelsQueryModel query)
        {
            reelsQuery = reelsQuery
                    .Where(x => x.Brand.Name.ToLower() == query.Brand.ToLower())
                    .ToList();

            return reelsQuery;
        }

        public ICollection<ReelListingViewModel> GetReelsByPage(ICollection<Reel> reelsQuery, AllReelsQueryModel query)
        {
            var reelsByPage = reelsQuery
                .Skip((query.currentPage -1) * AllReelsQueryModel.ReelsPerPage)
                .Take(AllReelsQueryModel.ReelsPerPage)
                .Select(x => new ReelListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            return reelsByPage;
        }

        public ICollection<Reel> GetReelsBySearchTerm(ICollection<Reel> reelsQuery, AllReelsQueryModel query)
        {
            reelsQuery = reelsQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();

            return reelsQuery;
        }

        public ICollection<Reel> GetReelsBySortTerm(ICollection<Reel> reelsQuery, AllReelsQueryModel query)
        {
            reelsQuery = query.Sorting switch
            {
                ReelSorting.MinPrice => reelsQuery.OrderBy(x => x.Price).ToList(),
                ReelSorting.MaxPrice => reelsQuery.OrderByDescending(x => x.Price).ToList(),
                ReelSorting.BrandAndModel => reelsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => reelsQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            return reelsQuery;
        }
    }
}
