using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Rods;
using System.Collections.Generic;

namespace DreamFishingNew.Services.Rods
{
    public interface IRodService
    {
        ICollection<Rod> GetAllRods();

        ICollection<Rod> GetRodsByBrand( ICollection<Rod> rodsQuery, AllRodsQueryModel query);

        ICollection<Rod> GetRodsBySearchTerm( ICollection<Rod> rodsQuery, AllRodsQueryModel query);

        ICollection<Rod> GetRodsBySortTerm( ICollection<Rod> rodsQuery, AllRodsQueryModel query);

        ICollection<RodListingViewModel> GetRodsByPage( ICollection<Rod> rodsQuery, AllRodsQueryModel query);

        ICollection<string> GetRodBrands();

        Brand GetRodBrandByName(AddRodFormModel rod);

        void CreateRod(AddRodFormModel rod, Brand brand);

        Rod GetRodById(int id);

        AddRodFormModel GetRodEditModel(int id);

        void EditRod(int id, AddRodFormModel rod);

        void DeleteRod(Rod rod);

        void DecrementRodQuantity(Rod currRod);
    }
}
