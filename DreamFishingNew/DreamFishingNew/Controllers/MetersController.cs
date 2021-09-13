using DreamFishingNew.Models.Meters;
using DreamFishingNew.Models.Shared;
using DreamFishingNew.Services.Meters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;
    public class MetersController : Controller
    {
        private IMeterService meterService;
        public MetersController(IMeterService meterService)
        {
            this.meterService = meterService;
        }

        public IActionResult All([FromQuery] AllMetersQueryModel query)
        {
            var metersQuery = meterService.GetAllMeters();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                metersQuery = meterService.GetMetersByBrand(metersQuery, query);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                metersQuery = meterService.GetMetersBySearchTerm(metersQuery, query);
            }

            metersQuery = meterService.GetMetersBySortTerm(metersQuery, query);

            var metersByPage = meterService.GetMetersByPage(metersQuery, query);

            var meterBrands = meterService.GetMeterBrands();

            var model = new AllMetersQueryModel
            {
                Brand = query.Brand,
                Brands = meterBrands,
                Meters = metersByPage,
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
        public IActionResult Add(AddMeterFormModel meter)
        {
            if (!ModelState.IsValid)
            {
                return View(meter);
            }

            var brand = meterService.GetMeterBrandByName(meter);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(meter.Brand), "Brand does not exist.");
            }

            meterService.CreateMeter(meter, brand);

            return RedirectToAction("Add", "Meters");
        }

        public IActionResult Details(int id)
        {
            var meter = meterService.GetMeterById(id);

            var model = new MeterDetailsViewModel
            {
                Id = meter.Id,
                Model = meter.Model,
                Brand = meter.Brand.Name,
                Image = meter.Image,
                Weight = meter.Weight,
                Description = meter.Description,
                Length = meter.Length,
                Price = meter.Price,
                Quantity = meter.Quantity
            };

            return View(model);
        }

        [Authorize]
        public IActionResult AddtoCart(int id)
        {
            var currMeter = meterService.GetMeterById(id);

            meterService.DecrementMeterQuantity(currMeter);

            var meterModel = new AddtoCartViewModel
            {
                Model = currMeter.Model,
                Brand = currMeter.Brand.Name,
                Image = currMeter.Image,
                Quantity = currMeter.Quantity
            };

            return View(meterModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = meterService.GetMeterEditModel(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddMeterFormModel item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var brand = meterService.GetMeterBrandByName(item);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }

            meterService.EditMeter(id, item);

            return RedirectToAction("All", "Meters");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var meter = meterService.GetMeterById(id);

            meterService.DeleteMeter(meter);

            return RedirectToAction("All", "Meters");
        }
    }
}
