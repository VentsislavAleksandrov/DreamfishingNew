using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Bags;
using System.Linq;

namespace DreamFishingNew.Services.Brands
{
    public class BrandService : IBrandService
    {
        private ApplicationDbContext data;

        public BrandService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public Brand GetBrand(AddBagFormModel bag)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == bag.Brand.ToLower());

            return brand;
        }

        public Brand GetBrandByFormModel(AddBagFormModel item)
        {
            var brand = data
                .Brands
                .FirstOrDefault(x => x.Name.ToLower() == item.Brand.ToLower());

            return brand;
        }

    }
}
