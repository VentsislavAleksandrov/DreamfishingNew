using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Clothes;
using System.Collections.Generic;

namespace DreamFishingNew.Services.Cloth
{
    public interface IClothService
    {
        ICollection<Clothes> GetAllClothes();

        ICollection<Clothes> GetClothesByBrand(ICollection<Clothes> clothesQuery, AllClothesQueryModel query);

        ICollection<Clothes> GetClothesBySerchrTerm(ICollection<Clothes> clothesQuery, AllClothesQueryModel query);

        ICollection<Clothes> GetClothesBySortTerm(ICollection<Clothes> clothesQuery, AllClothesQueryModel query);

        ICollection<ClothesListingViewModel> GetClothesByPage(ICollection<Clothes> clothesQuery, AllClothesQueryModel query);

        ICollection<string> GetClothesBrands();

        Brand GetClothesBrandByName(AddClothesFormModel clothes);

        void CreateClothes(AddClothesFormModel clothes, Brand brand);

        Clothes GetClothesById(int id);

        AddClothesFormModel GetClothesEditModel(int id);

        void EditClothes(int id, AddClothesFormModel item);

        void DeleteClothes(Clothes clothes);

        void DecrementClothesQuantity(Clothes currClothes);



    }
}
