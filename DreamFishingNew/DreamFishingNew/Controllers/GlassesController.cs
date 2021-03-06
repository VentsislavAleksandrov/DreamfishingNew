using DreamFishingNew.Models.Glasses;
using DreamFishingNew.Models.Shared;
using DreamFishingNew.Services.Glass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    using static WebConstants;

    public class GlassesController: Controller
    {        
        private IGlassService glassService;

        public GlassesController( IGlassService glassService)
        {            
            this.glassService = glassService;
        }

        public IActionResult All([FromQuery]AllGlassesQueryModel query)
        {
            var glassesQuery = glassService.GetAllGlasses();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                glassesQuery = glassService.GetGlassesByBrand(query, glassesQuery);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                glassesQuery = glassService.GetGlassesBySearchTerm(query, glassesQuery);
            }

            glassesQuery = glassService.GetGlassesBySortTerm(query, glassesQuery);

            var glassesByPage = glassService.GetGlassesByPage(query, glassesQuery);

            var glassesBrands = glassService.GetGlassesBrands();

            var model = new AllGlassesQueryModel
            {
                Brand = query.Brand,
                Brands = glassesBrands,
                Glasses = glassesByPage,
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
        public IActionResult Add(AddGlassesFormModel glasses)
        {            
            if (!ModelState.IsValid)
            {
                return View(glasses);
            }

            var brand = glassService.GetGlassesBrandByName(glasses);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(glasses.Brand), "Brand does not exist.");
            }

            glassService.CreateGlasses(glasses, brand);

            return RedirectToAction("Add", "Glasses");
        }

        public IActionResult Details(int id)
        {
            var glasses = glassService.GetGlassesById(id);

            var model = new GlassesDetailsViewModel
            {
                Id = glasses.Id,
                Model = glasses.Model,
                Brand = glasses.Brand.Name,
                Image = glasses.Image,
                Weight = glasses.Weight,
                Description = glasses.Description,
                Price = glasses.Price,
                Quantity = glasses.Quantity
            };

            return View(model);
        }

        [Authorize]
        public IActionResult AddtoCart(int id)
        {
            var currGlasses = glassService.GetGlassesById(id);

            glassService.DecrementGlassesQuantity(currGlasses);

            var glassesModel = new AddtoCartViewModel
            {
                Model = currGlasses.Model,
                Brand = currGlasses.Brand.Name,
                Image = currGlasses.Image, 
                Quantity = currGlasses.Quantity
            };
          
            return View(glassesModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = glassService.GetGlassesEditFormModel(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int id, AddGlassesFormModel item)
        {            
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var brand = glassService.GetGlassesBrandByName(item);

            if (brand == null)
            {
                this.ModelState.AddModelError(nameof(item.Brand), "Brand does not exist.");
            }

            glassService.EditGlasses(id, item);

            return RedirectToAction("All", "Glasses");
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var glasses = glassService.GetGlassesById(id);

            glassService.DeleteGlasses(glasses);

            return RedirectToAction("All", "Glasses");
        }
    }
}
