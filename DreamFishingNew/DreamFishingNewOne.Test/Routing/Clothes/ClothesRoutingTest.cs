using DreamFishingNew.Controllers;
using DreamFishingNew.Models.Clothes;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace DreamFishingNewOne.Test.Routing.Clothes
{
    public class ClothesRoutingTest
    {
        [Fact]
        public void AllRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Clothes/All")
            .To<ClothesController>(x => x.All(With.Any<AllClothesQueryModel>()))
            ;

        [Fact]
        public void PostAddRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Clothes/Add")
            .WithMethod(HttpMethod.Post))
            .To<ClothesController>(x => x.Add());


        [Fact]
        public void GetAddRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Clothes/Add")
            .To<ClothesController>(x => x.Add());

        [Fact]
        public void DetailsRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Clothes/Details/1")
            .To<ClothesController>(x => x.Details(1));

        [Fact]
        public void AddToCartRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Clothes/AddtoCart/1")
            .To<ClothesController>(x => x.AddtoCart(1));

        [Fact]
        public void GetEditRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Clothes/Edit/1")
            .To<ClothesController>(x => x.Edit(1));

        [Fact]
        public void PostEditRouteSouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Clothes/Edit/1")
            .WithMethod(HttpMethod.Post))
            .To<ClothesController>(x => x.Edit(1));

        [Fact]
        public void DeleteRouteShouldMatch()
            => MyRouting
            .Configuration()
            .ShouldMap("/Clothes/Delete/1")
            .To<ClothesController>(x => x.Delete(1));
    }
}
