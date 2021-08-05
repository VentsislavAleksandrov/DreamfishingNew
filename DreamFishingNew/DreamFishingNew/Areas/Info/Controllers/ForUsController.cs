using Microsoft.AspNetCore.Mvc;

namespace DreamFishingNew.Areas.Info.Controllers
{
    [Area("Info")]
    public class ForUsController: Controller
    {
        public IActionResult Index() 
        {
            return View();
        }
    }
}
