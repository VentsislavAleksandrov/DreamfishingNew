using DreamFishingNew.Controllers;
using DreamFishingNew.Models.Rods;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Routing.Rods
{
    public class RodRouteTest
    {
        [Fact]
        public void AllRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Rods/All")
            .To<RodsController>(x => x.All(With.Any<AllRodsQueryModel>()))
            ;

        [Fact]
        public void PostAddRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Rods/Add")
            .WithMethod(HttpMethod.Post))
            .To<RodsController>(x => x.Add());

        [Fact]
        public void GetAddRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Rods/Add")
            .To<RodsController>(x => x.Add());

        [Fact]
        public void DetailsRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Rods/Details/1")
            .To<RodsController>(x => x.Details(1));

        [Fact]
        public void AddToCartRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Rods/AddtoCart/1")
            .To<RodsController>(x => x.AddtoCart(1));

        [Fact]
        public void GetEditRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Rods/Edit/1")
            .To<RodsController>(x => x.Edit(1));

        [Fact]
        public void PostEditRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Rods/Edit/1")
            .WithMethod(HttpMethod.Post))
            .To<RodsController>(x => x.Edit(1));

        [Fact]
        public void DeleteRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Rods/Delete/1")
            .To<RodsController>(x => x.Delete(1));
    }
}
