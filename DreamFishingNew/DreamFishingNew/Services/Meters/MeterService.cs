using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Meters;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DreamFishingNew.Services.Meters
{
    public class MeterService: IMeterService
    {
        private ApplicationDbContext data;

        public MeterService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void CreateMeter(AddMeterFormModel meter, Brand brand)
        {
            var currentMeter = new Meter
            {
                Model = meter.Model,
                BrandId = brand.Id,
                Image = meter.Image,
                Price = meter.Price,
                Weight = meter.Weight,
                Length = meter.Length,
                Description = meter.Description,
                Quantity = meter.Quantity
            };

            data.Meters.Add(currentMeter);
            data.SaveChanges();
        }

        public void DeleteMeter(Meter meter)
        {
            data.Meters.Remove(meter);
            data.SaveChanges();
        }

        public void EditMeter(int id, AddMeterFormModel item)
        {
            var meter = data
                .Meters
                .Include("Brand")
                .Where(x => x.Id == id)
                .FirstOrDefault();

            meter.Model = item.Model;
            meter.Brand.Name = item.Brand;
            meter.Description = item.Description;
            meter.Image = item.Image;
            meter.Length = item.Length;
            meter.Price = item.Price;
            meter.Quantity = item.Quantity;
            meter.Weight = item.Weight;

            data.SaveChanges();
        }

        public ICollection<Meter> GetAllMeters()
        {
             var metersQuery = data.Meters
                .Include("Brand")
                .ToList();

            return metersQuery;
        }

        public Brand GetMeterBrandByName(AddMeterFormModel meter)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == meter.Brand.ToLower());

            return brand;
        }

        public ICollection<string> GetMeterBrands()
        {
            var meterBrands = data
                .Meters
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();

            return meterBrands;
        }

        public Meter GetMeterById(int id)
        {
            var meter = data
                .Meters
                .Include("Brand")
                .FirstOrDefault(x => x.Id == id);

            return meter;
        }

        public AddMeterFormModel GetMeterEditModel(int id)
        {
            var model = data.Meters
                .Where(x => x.Id == id)
                .Select(x => new AddMeterFormModel 
                {
                Model = x.Model,
                Brand = x.Brand.Name,
                Length = x.Length,
                Weight = x.Weight,
                Description = x.Description,
                Image = x.Image,
                Price = x.Price,
                Quantity = x.Quantity
                })
                .FirstOrDefault();

            return model;
        }

        public ICollection<Meter> GetMetersByBrand(ICollection<Meter> metersQuery, AllMetersQueryModel query)
        {
            metersQuery = metersQuery
                    .Where(x => x.Brand.Name.ToLower() == query.Brand.ToLower())
                    .ToList();

            return metersQuery;
        }

        public ICollection<MeterListingViewModel> GetMetersByPage(ICollection<Meter> metersQuery, AllMetersQueryModel query)
        {
            var meters = metersQuery
                .Skip((query.currentPage -1) * AllMetersQueryModel.MetersPerPage)
                .Take(AllMetersQueryModel.MetersPerPage)
                .Select(x => new MeterListingViewModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Brand = x.Brand.Name,
                    Image = x.Image,
                    Price = x.Price,
                })
                .ToList();

            return meters;
        }

        public ICollection<Meter> GetMetersBySearchTerm(ICollection<Meter> metersQuery, AllMetersQueryModel query)
        {
            metersQuery = metersQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();

            return metersQuery;
        }

        public ICollection<Meter> GetMetersBySortTerm(ICollection<Meter> metersQuery, AllMetersQueryModel query)
        {
            metersQuery = query.Sorting switch
            {
                MeterSorting.MinPrice => metersQuery.OrderBy(x => x.Price).ToList(),
                MeterSorting.MaxPrice => metersQuery.OrderByDescending(x => x.Price).ToList(),
                MeterSorting.BrandAndModel => metersQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => metersQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

            return metersQuery;
        }
    }
}
