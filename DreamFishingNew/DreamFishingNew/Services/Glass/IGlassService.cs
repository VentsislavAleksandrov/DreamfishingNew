using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Glasses;
using System.Collections.Generic;

namespace DreamFishingNew.Services.Glass
{
    public interface IGlassService
    {
        ICollection<Glasses> GetAllGlasses();

        ICollection<Glasses> GetGlassesByBrand(AllGlassesQueryModel query, ICollection<Glasses> glassesQuery);

        ICollection<Glasses> GetGlassesBySearchTerm(AllGlassesQueryModel query, ICollection<Glasses> glassesQuery);

        ICollection<Glasses> GetGlassesBySortTerm(AllGlassesQueryModel query, ICollection<Glasses> glassesQuery);

        ICollection<GlassesListingViewModel> GetGlassesByPage(AllGlassesQueryModel query, ICollection<Glasses> glassesQuery);

        ICollection<string> GetGlassesBrands();

        Brand GetGlassesBrandByName(AddGlassesFormModel glasses);

        void CreateGlasses(AddGlassesFormModel glasses, Brand brand);

        Glasses GetGlassesById(int id);

        AddGlassesFormModel GetGlassesEditFormModel(int id);

        void EditGlasses(int id, AddGlassesFormModel item);

        void DeleteGlasses(Glasses glasses);

    }
}
