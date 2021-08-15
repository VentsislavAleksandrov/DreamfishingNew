using DreamFishingNew.Controllers;
using DreamFishingNew.Models.Bags;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Routing.Bags
{
    public class BagRoutingTest
    {
        

        [Fact]
        public void AllRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Bags/All")
            .To<BagsController>(x => x.All(With.Any<AllBagsQueryModel>()))
            ;

        [Fact]
        public void PostAddRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Bags/Add")
            .WithMethod(HttpMethod.Post))
            .To<BagsController>(x => x.Add());


        [Fact]
        public void GetAddRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Bags/Add")
            .To<BagsController>(x => x.Add());

        [Fact]
        public void DetailsRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Bags/Details/1")
            .To<BagsController>(x => x.Details(1));

        [Fact]
        public void AddToCartRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Bags/AddtoCart/1")
            .To<BagsController>(x => x.AddtoCart(1));

        [Fact]
        public void GetEditRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Bags/Edit/1")
            .To<BagsController>(x => x.Edit(1));

        [Fact]
        public void PostEditRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Bags/Edit/1")
            .WithMethod(HttpMethod.Post))
            .To<BagsController>(x => x.Edit(1));

        [Fact]
        public void DeleteRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Bags/Delete/1")
            .To<BagsController>(x => x.Delete(1));


    }
}
