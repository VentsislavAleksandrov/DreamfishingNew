using DreamFishingNew.Controllers;
using DreamFishingNew.Models.Lines;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Routing.Lines
{
    public class LineRouteTest
    {
        [Fact]
        public void AllRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Lines/All")
            .To<LinesController>(x => x.All(With.Any<AllLinesQueryModel>()))
            ;

        [Fact]
        public void PostAddRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Lines/Add")
            .WithMethod(HttpMethod.Post))
            .To<LinesController>(x => x.Add());


        [Fact]
        public void GetAddRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Lines/Add")
            .To<LinesController>(x => x.Add());

        [Fact]
        public void DetailsRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Lines/Details/1")
            .To<LinesController>(x => x.Details(1));

        [Fact]
        public void AddToCartRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Lines/AddtoCart/1")
            .To<LinesController>(x => x.AddtoCart(1));

        [Fact]
        public void GetEditRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Lines/Edit/1")
            .To<LinesController>(x => x.Edit(1));

        [Fact]
        public void PostEditRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Lines/Edit/1")
            .WithMethod(HttpMethod.Post))
            .To<LinesController>(x => x.Edit(1));

        [Fact]
        public void DeleteRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Lines/Delete/1")
            .To<LinesController>(x => x.Delete(1));
    }
}
