using DreamFishingNew.Controllers;
using DreamFishingNew.Models.Reels;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Routing.Reels
{
    public class ReelRouteTest
    {
        [Fact]
        public void AllRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Reels/All")
            .To<ReelsController>(x => x.All(With.Any<AllReelsQueryModel>()))
            ;

        [Fact]
        public void PostAddRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Reels/Add")
            .WithMethod(HttpMethod.Post))
            .To<ReelsController>(x => x.Add());

        [Fact]
        public void GetAddRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Reels/Add")
            .To<ReelsController>(x => x.Add());

        [Fact]
        public void DetailsRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Reels/Details/1")
            .To<ReelsController>(x => x.Details(1));

        [Fact]
        public void AddToCartRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Reels/AddtoCart/1")
            .To<ReelsController>(x => x.AddtoCart(1));

        [Fact]
        public void GetEditRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Reels/Edit/1")
            .To<ReelsController>(x => x.Edit(1));

        [Fact]
        public void PostEditRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Reels/Edit/1")
            .WithMethod(HttpMethod.Post))
            .To<ReelsController>(x => x.Edit(1));

        [Fact]
        public void DeleteRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Reels/Delete/1")
            .To<ReelsController>(x => x.Delete(1));
    }
}
