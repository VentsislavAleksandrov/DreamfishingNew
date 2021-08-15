using DreamFishingNew.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNew.Test.Home
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShuldRerurnView()
        => MyMvc
            .Pipeline()
            .ShouldMap("/")
            .To<HomeController>(x => x.Index())
            .Which()
            .ShouldReturn()
            .View();
    }
}
