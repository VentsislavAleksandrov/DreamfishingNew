using DreamFishingNew.Models.Lines;
using DreamFishingNew.Models.Shared;
using DreamFishingNew.Services.Lines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class LinesController : Controller
    {
        private ILineService lineService;
        public LinesController(ILineService lineService)
        {
            this.lineService = lineService;
        }

        public IActionResult All([FromQuery] AllLinesQueryModel query)
        {
            var linesQuery = lineService.GetAllLines();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                linesQuery = lineService.GetLinesByBrand(linesQuery, query);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                linesQuery = linesQuery
                    .Where(x => (x.Brand.Name + " " + x.Model).ToLower()
                    .Contains(query.SearchTerm.ToLower()) || x.Description.ToLower()
                    .Contains(query.SearchTerm.ToLower()))
                    .ToList();
            }

            linesQuery = lineService.GetLinesBySortTerm(linesQuery, query);

            var linesByPage = lineService.GetLinesByPage(linesQuery, query);

            var lineBrands = lineService.GetLineBrands();


            var model = new AllLinesQueryModel
            {
                Brand = query.Brand,
                Brands = lineBrands,
                Lines = linesByPage,
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
        public IActionResult Add(AddLineFormModel line)
        {
            if (!ModelState.IsValid)
            {
                return View(line);
            }

            var brand = lineService.GetLineBrandByName(line);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(line.Brand), "Brand does not exist.");
            }

            lineService.CreateLine(line, brand);

            return RedirectToAction("Add", "Lines");
        }

        public IActionResult Details(int id)
        {
            var line = lineService.GetLineById(id);

            var model = new LineDetailsViewModel
            {
                Id = line.Id,
                Model = line.Model,
                Brand = line.Brand.Name,
                Image = line.Image,
                Length = line.Length,
                Size = line.Size,
                Weight = line.Weight,
                Description = line.Description,
                Price = line.Price,
                Quantity = line.Quantity
            };

            return View(model);
        }

        [Authorize]
        public IActionResult AddtoCart(int id)
        {
            var currLine = lineService.GetLineById(id);

            lineService.DecrementLineQuantity(currLine);

            var lineModel = new AddtoCartViewModel
            {
                Model = currLine.Model,
                Brand = currLine.Brand.Name,
                Image = currLine.Image,
                Quantity = currLine.Quantity
            };

            return View(lineModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = lineService.GetLineEditModel(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddLineFormModel item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var brand = lineService.GetLineBrandByName(item);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }

            lineService.EditLine(id, item);

            return RedirectToAction("All", "Lines");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var line = lineService.GetLineById(id);

            lineService.DeleteLine(line);

            return RedirectToAction("All", "Lines");
        }
    }
}
