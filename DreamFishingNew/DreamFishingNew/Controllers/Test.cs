using DreamFishingNew.Data;
using DreamFishingNew.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DreamFishingNew.Controllers
{
    public class TestController: Controller
    {
        private ApplicationDbContext data;

        public TestController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public void GetPropertyOfUser()
        {
            var UserId = this.User.Id();

            var currUser = data.Users.Where(x => x.Id == UserId);

            var pk = currUser.Select(x => x.ProductCart).FirstOrDefault();
        }

    }
}
