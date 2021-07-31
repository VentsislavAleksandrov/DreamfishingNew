using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Bags;

namespace DreamFishingNew.Services.Brands
{
    public interface IBrandService
    {
        Brand GetBrand(AddBagFormModel bag);

        Brand GetBrandByFormModel(AddBagFormModel item);
    }
}
