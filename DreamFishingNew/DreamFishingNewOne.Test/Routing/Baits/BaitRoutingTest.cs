using DreamFishingNew.Controllers;
using DreamFishingNew.Models.Baits;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Routing.Baits
{
    public class BaitRoutingTest
    {
        [Fact]
        public void AllRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Baits/All")
            .To<BaitsController>(x => x.All(With.Any<AllBaitsQueryModel>()))
            ;

        [Fact]
        public void PostAddRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Baits/Add")
            .WithMethod(HttpMethod.Post))
            .To<BaitsController>(x => x.Add());


        [Fact]
        public void GetAddRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Baits/Add")
            .To<BaitsController>(x => x.Add());

        [Fact]
        public void DetailsRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Baits/Details/1")
            .To<BaitsController>(x => x.Details(1));

        [Fact]
        public void AddToCartRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Baits/AddtoCart/1")
            .To<BaitsController>(x => x.AddtoCart(1));

        [Fact]
        public void GetEditRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Baits/Edit/1")
            .To<BaitsController>(x => x.Edit(1));

        [Fact]
        public void PostEditRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Baits/Edit/1")
            .WithMethod(HttpMethod.Post))
            .To<BaitsController>(x => x.Edit(1));

        [Fact]
        public void DeleteRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Baits/Delete/1")
            .To<BaitsController>(x => x.Delete(1));
    }
}
