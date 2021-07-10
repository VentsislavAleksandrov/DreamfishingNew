using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    public class ClothesController: Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
