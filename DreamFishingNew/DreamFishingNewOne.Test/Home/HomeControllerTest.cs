using DreamFishingNew.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Home
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap("/")
            .To<HomeController>(x => x.Index())
            .Which()
            .ShouldReturn()
            .View();
    }
}
