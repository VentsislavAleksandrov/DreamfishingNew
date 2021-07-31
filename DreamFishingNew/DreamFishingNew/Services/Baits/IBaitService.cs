using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Baits;
using System.Collections.Generic;

namespace DreamFishingNew.Services.Baits
{
    public interface IBaitService
    {
        ICollection<Bait> GetAllBaits();

        ICollection<Bait> GetBaitsByBrand(ICollection<Bait> baits, AllBaitsQueryModel query);

        ICollection<Bait> GetBaitsBySearchTerm(ICollection<Bait> baits, AllBaitsQueryModel query);

        ICollection<Bait> GetBaitsBySortTerm(BaitSorting sorting, ICollection<Bait> baitsQuery);

        ICollection<BaitListingViewModel> GetBaitsByPage(ICollection<Bait> baitsQuery, AllBaitsQueryModel query);

        ICollection<string> GetBiteBrands();

        Brand GetBaitBrandByName(AddBaitFormModel bait);

        void CreateBait(AddBaitFormModel bait, Brand brand);

        Bait GetBaitById(int id);

        AddBaitFormModel GetBaitEditModel(int id);

        void EditBait(int id, AddBaitFormModel item);

        void DeleteBait(Bait bait);
    }
}
