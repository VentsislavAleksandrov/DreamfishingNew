using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Clothes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DreamFishingNew.Services.Cloth
{
    public class ClothService : IClothService
    {
        private ApplicationDbContext data;

        public ClothService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void CreateClothes(AddClothesFormModel clothes, Brand brand)
        {
            var currentClothes = new Clothes
            {
                Model = clothes.Model,
                BrandId = brand.Id,
                Image = clothes.Image,
                Price = clothes.Price,
                Weight = clothes.Weight,
                Size = clothes.Size,
                Waterproof = clothes.Waterproof.ToLower() == "yes",
                Material = clothes.Material,
                Description = clothes.Description,
                Quantity = clothes.Quantity
            };

            data.Clothes.Add(currentClothes);
            data.SaveChanges();

            
        }

        public void DecrementClothesQuantity(Clothes currClothes)
        {
            currClothes.Quantity--;

            if (currClothes.Quantity < 0)
            {
                currClothes.Quantity = 0;
            }

            data.SaveChanges();
        }

        public void DeleteClothes(Clothes clothes)
        {
            data.Clothes.Remove(clothes);
            data.SaveChanges();
        }

        public void EditClothes(int id, AddClothesFormModel item)
        {
            var clothes = data
                .Clothes
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            clothes.Model = item.Model;
            clothes.Brand.Name = item.Brand;
            clothes.Size = item.Size;
            clothes.Description = item.Description;
            clothes.Image = item.Image;
            clothes.Waterproof = item.Waterproof.ToLower() == "yes" ? true : false;
            clothes.Price = item.Price;
            clothes.Quantity = item.Quantity;
            clothes.Weight = item.Weight;
            clothes.Material = item.Material;

            data.SaveChanges();
        }

        public ICollection<Clothes> GetAllClothes()
        {
            var clothesQuery = data.Clothes
                .Include("Brand")
                .ToList();

            return clothesQuery;
        }

        public Brand GetClothesBrandByName(AddClothesFormModel clothes)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == clothes.Brand.ToLower());

            return brand;
        }

        public ICollection<string> GetClothesBrands()
        {
            var clothesBrands = data
                .Clothes
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();

            return clothesBrands;
        }

        public ICollection<Clothes> GetClothesByBrand(ICollection<Clothes> clothesQuery, AllClothesQueryModel query)
        {
            clothesQuery = clothesQuery
                    .Where(x => x.Brand.Name.ToLower() == query.Brand.ToLower())
                    .ToList();

            return clothesQuery;
        }

        public Clothes GetClothesById(int id)
        {
            var clothes = data
                .Clothes
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            return clothes;
        }

        public ICollection<ClothesListingViewModel> GetClothesByPage(ICollection<Clothes> clothesQuery, AllClothesQueryModel query)
        {
            var clothes = clothesQuery
                .Skip((query.currentPage -1) * AllClothesQueryModel.ClothesPerPage)
                .Take(AllClothesQueryModel.ClothesPerPage)
                .Select(x => new ClothesListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            return clothes;
        }

        public ICollection<Clothes> GetClothesBySerchrTerm(ICollection<Clothes> clothesQuery, AllClothesQueryModel query)
        {
            clothesQuery = clothesQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();

            return clothesQuery;
        }

        public ICollection<Clothes> GetClothesBySortTerm(ICollection<Clothes> clothesQuery, AllClothesQueryModel query)
        {
            clothesQuery = query.Sorting switch
            {
                ClothesSorting.MinPrice => clothesQuery.OrderBy(x => x.Price).ToList(),
                ClothesSorting.MaxPrice => clothesQuery.OrderByDescending(x => x.Price).ToList(),
                ClothesSorting.BrandAndModel => clothesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => clothesQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            return clothesQuery;
        }

        public AddClothesFormModel GetClothesEditModel(int id)
        {
            var model = data.Clothes
                .Where(x => x.Id == id)
                .Select(x => new AddClothesFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                Material = x.Material,
                Size = x.Size,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Price = x.Price,
                Quantity = x.Quantity,
                Waterproof = x.Waterproof ? "yes" : "no"
                })
                .FirstOrDefault();

            return model;
        }
    }
}
