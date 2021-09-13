using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Glasses;
using DreamFishingNew.Services.Glass;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DreamFishingNew.Services.Glass

{
    public class GlassService : IGlassService
    {
        private ApplicationDbContext data;

        public GlassService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void CreateGlasses(AddGlassesFormModel glasses, Brand brand)
        {
            var currentGlasses = new Glasses
            {
                Model = glasses.Model,
                BrandId = brand.Id,
                Image = glasses.Image,
                Price = glasses.Price,
                Weight = glasses.Weight,
                Description = glasses.Description,
                Quantity = glasses.Quantity
            };

            data.Glasses.Add(currentGlasses);
            data.SaveChanges();
        }

        public ICollection<Glasses> GetAllGlasses()
        {
            var glassesQuery = data.Glasses
                .Include("Brand")
                .ToList();

            return glassesQuery;
        }

        public Brand GetGlassesBrandByName(AddGlassesFormModel glasses)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == glasses.Brand.ToLower());

            return brand;
        }

        public ICollection<string> GetGlassesBrands()
        {
            var glassesBrands = data
                .Glasses
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();

            return glassesBrands;
        }

        public Glasses GetGlassesById(int id)
        {
            var glasses = data
                .Glasses
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            return glasses;
        }

        public ICollection<Glasses> GetGlassesByBrand(AllGlassesQueryModel query, ICollection<Glasses> glassesQuery)
        {
            glassesQuery = data.Glasses
                    .Where(x => x.Brand.Name.ToLower() == query.Brand)
                    .ToList();

            return glassesQuery;
        }

        public ICollection<GlassesListingViewModel> GetGlassesByPage(AllGlassesQueryModel query, ICollection<Glasses> glassesQuery)
        {
            var glassesByPage = glassesQuery
                 .Skip((query.currentPage - 1) * AllGlassesQueryModel.GlassesPerPage)
                 .Take(AllGlassesQueryModel.GlassesPerPage)
                 .Select(x => new GlassesListingViewModel
                 {
                     Id = x.Id,
                     Model = x.Model,
                     Brand = x.Brand.Name,
                     Image = x.Image,
                     Price = x.Price,
                 })
                 .ToList();

            return glassesByPage;
        }

        public ICollection<Glasses> GetGlassesBySearchTerm(AllGlassesQueryModel query, ICollection<Glasses> glassesQuery)
        {
            glassesQuery = glassesQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower()
                    .Contains(query.SearchTerm.ToLower()) || x.Description.ToLower()
                    .Contains(query.SearchTerm.ToLower()))
                    .ToList();

            return glassesQuery;
        }

        public ICollection<Glasses> GetGlassesBySortTerm(AllGlassesQueryModel query, ICollection<Glasses> glassesQuery)
        {
            glassesQuery = query.Sorting switch
            {
                GlassesSorting.MinPrice => glassesQuery.OrderBy(x => x.Price).ToList(),
                GlassesSorting.MaxPrice => glassesQuery.OrderByDescending(x => x.Price).ToList(),
                GlassesSorting.BrandAndModel => glassesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => glassesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            return glassesQuery;
        }

        public AddGlassesFormModel GetGlassesEditFormModel(int id)
        {
            var model = data.Glasses
               .Where(x => x.Id == id)
               .Select(x => new AddGlassesFormModel
               {
                   Model = x.Model,
                   Brand = x.Brand.Name,
                   Weight = x.Weight,
                   Description = x.Description,
                   Image = x.Image,
                   Price = x.Price,
                   Quantity = x.Quantity
               })
               .FirstOrDefault();

            return model;
        }

        public void EditGlasses(int id, AddGlassesFormModel item)
        {
            var glasses = data
                .Glasses
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            glasses.Model = item.Model;
            glasses.Brand.Name = item.Brand;
            glasses.Description = item.Description;
            glasses.Image = item.Image;
            glasses.Price = item.Price;
            glasses.Quantity = item.Quantity;
            glasses.Weight = item.Weight;

            data.SaveChanges();
        }

        public void DeleteGlasses(Glasses glasses)
        {
            data.Glasses.Remove(glasses);
            data.SaveChanges();
        }

        public void DecrementGlassesQuantity(Glasses currGlasses)
        {
            currGlasses.Quantity--;

            if (currGlasses.Quantity < 0)
            {
                currGlasses.Quantity = 0;
            }

            data.SaveChanges();
        }
    }
}
