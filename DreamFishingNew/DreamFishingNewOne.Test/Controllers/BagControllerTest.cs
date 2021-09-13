using DreamFishingNew.Controllers;
using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Bags;
using DreamFishingNew.Services.Bags;
using Moq;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Controllers
{
    
    public class BagControllerTest
    {
        

        [Fact]
        public void GetAddShouldReturrnView()
            => MyController<BagsController>
                .Instance()
                .Calling(x => x.Add())
                .ShouldReturn()
                .View();

        [Fact]
        public void DetailsShouldReturnCorectView()
        {

            var bag = new Bag
            {
                Id = 1,
                Image = null,
                BrandId = 1,
                Description = "descriprion",
                Model = "model",
                Price = 1,
                Quantity = 1,
                Size = "L",
                Weight = 1

            };
            
            MyController<BagsController>
                  .Instance(x => x
                  .WithData(bag))
                  .Calling(x => x.Details(1))
                  .ShouldReturn()
                  .View(x => x
                  .WithModelOfType<BagDetailsViewModel>());
        }    

        

    }
}


