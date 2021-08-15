using DreamFishingNew.Controllers;
using DreamFishingNew.Models.Meters;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Routing.Meters
{
    public class MeterRouteTest
    {
        [Fact]
        public void AllRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Meters/All")
            .To<MetersController>(x => x.All(With.Any<AllMetersQueryModel>()))
            ;

        [Fact]
        public void PostAddRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Meters/Add")
            .WithMethod(HttpMethod.Post))
            .To<MetersController>(x => x.Add());


        [Fact]
        public void GetAddRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Meters/Add")
            .To<MetersController>(x => x.Add());

        [Fact]
        public void DetailsRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Meters/Details/1")
            .To<MetersController>(x => x.Details(1));

        [Fact]
        public void AddToCartRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Meters/AddtoCart/1")
            .To<MetersController>(x => x.AddtoCart(1));

        [Fact]
        public void GetEditRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Meters/Edit/1")
            .To<MetersController>(x => x.Edit(1));

        [Fact]
        public void PostEditRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Meters/Edit/1")
            .WithMethod(HttpMethod.Post))
            .To<MetersController>(x => x.Edit(1));

        [Fact]
        public void DeleteRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Meters/Delete/1")
            .To<MetersController>(x => x.Delete(1));
    }
}
