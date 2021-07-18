﻿using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Meters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class MetersController: Controller
    {
        private ApplicationDbContext data;

        public MetersController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery]AllMetersQueryModel query)
        {
            var metersQuery = data.Meters
                .Include<Meter>("Brand")
                .ToList();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                metersQuery = data.Meters
                    .Where(x => x.Brand.Name.ToLower() == query.Brand)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                metersQuery = metersQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower().Contains(query.SearchTerm.ToLower())
                    ||x.Description.ToLower().Contains(query.SearchTerm.ToLower())
                    )
                    .ToList();
            }

            metersQuery = query.Sorting switch
            {
                MeterSorting.MinPrice => metersQuery.OrderBy(x => x.Price).ToList(),
                MeterSorting.MaxPrice => metersQuery.OrderByDescending(x => x.Price).ToList(),
                MeterSorting.BrandAndModel => metersQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList(),
                _ => metersQuery.OrderBy(x => x.Brand.Name).ThenBy(x => x.Model).ToList()
            };

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

            


            var meterBrands = data
                .Meters
                .Select(x => x.Brand.Name)
                .Distinct()
                .ToList();


            var model = new AllMetersQueryModel
            {
                Brand = query.Brand,
                Brands = meterBrands,
                Meters = meters,
                Sorting = query.Sorting,
                SearchTerm = query.SearchTerm,
            };

            return View(model);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddMeterFormModel meter)
        {
            var brand = data.Brands.FirstOrDefault(x => x.Name.ToLower() == meter.Brand.ToLower());

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(meter.Brand), "Brand does not exist.");
            }


            if (!ModelState.IsValid)
            {
                return View(meter);
            }


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

            return RedirectToAction("Add", "Meters");
        }
    }
}
