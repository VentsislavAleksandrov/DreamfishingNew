using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Controllers
{
    public class LinesController: Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
