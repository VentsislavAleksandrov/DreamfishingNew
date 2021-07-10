using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    public class GlassesController: Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
