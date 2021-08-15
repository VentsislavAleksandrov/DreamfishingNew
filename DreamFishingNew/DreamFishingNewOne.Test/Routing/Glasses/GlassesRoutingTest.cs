using DreamFishingNew.Controllers;
using DreamFishingNew.Models.Glasses;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Routing.Glasses
{
    public class GlassesRoutingTest
    {
        [Fact]
        public void AllRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Glasses/All")
            .To<GlassesController>(x => x.All(With.Any<AllGlassesQueryModel>()))
            ;

        [Fact]
        public void PostAddRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Glasses/Add")
            .WithMethod(HttpMethod.Post))
            .To<GlassesController>(x => x.Add());


        [Fact]
        public void GetAddRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Glasses/Add")
            .To<GlassesController>(x => x.Add());

        [Fact]
        public void DetailsRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Glasses/Details/1")
            .To<GlassesController>(x => x.Details(1));

        [Fact]
        public void AddToCartRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Glasses/AddtoCart/1")
            .To<GlassesController>(x => x.AddtoCart(1));

        [Fact]
        public void GetEditRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Glasses/Edit/1")
            .To<GlassesController>(x => x.Edit(1));

        [Fact]
        public void PostEditRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Glasses/Edit/1")
            .WithMethod(HttpMethod.Post))
            .To<GlassesController>(x => x.Edit(1));

        [Fact]
        public void DeleteRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Glasses/Delete/1")
            .To<GlassesController>(x => x.Delete(1));
    }
}
