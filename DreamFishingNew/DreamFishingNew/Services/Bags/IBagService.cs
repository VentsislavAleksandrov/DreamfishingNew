using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Bags;
using System.Collections.Generic;

namespace DreamFishingNew.Services.Bags
{
    public interface IBagService
    {
        ICollection<Bag> GetAllBags();

        ICollection<Bag> GetBagsByBrand(AllBagsQueryModel query, ICollection<Bag> bagsQuery);

        ICollection<Bag> GetBagsBySearchTerm(string searchterm, ICollection<Bag> bagsQuery);

        ICollection<Bag> SortBagsBySortTerm(BagSorting sortTerm, ICollection<Bag> bagsQuery);

        ICollection<BagListingViewModel> GetBagsByPage(AllBagsQueryModel query, ICollection<Bag> bagsQuery);

        ICollection<string> GetBagBrands();

        void CreateBag(AddBagFormModel bag, Brand brand);

        Bag GetBagById(int id);

        AddBagFormModel GetBagEditFormModel(int id);

        void Editbag(Bag bag, AddBagFormModel item);

        void DeleteBag(Bag bag);

        Brand GetBagBrand(AddBagFormModel bag);

        void DecrementBagQuantity(Bag bag);
    }
}
