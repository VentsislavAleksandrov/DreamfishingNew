using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    public class BagsController: Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
