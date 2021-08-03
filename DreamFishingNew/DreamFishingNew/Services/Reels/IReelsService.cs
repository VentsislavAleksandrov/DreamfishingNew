using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Reels;
using System.Collections.Generic;

namespace DreamFishingNew.Services.Reels
{
    public interface IReelService
    {
        ICollection<Reel> GetAllReels();

        ICollection<Reel> GetReelsByBrand( ICollection<Reel> reelsQuery, AllReelsQueryModel query);

        ICollection<Reel> GetReelsBySearchTerm( ICollection<Reel> reelsQuery, AllReelsQueryModel query);

        ICollection<Reel> GetReelsBySortTerm( ICollection<Reel> reelsQuery, AllReelsQueryModel query);

        ICollection<ReelListingViewModel> GetReelsByPage( ICollection<Reel> reelsQuery, AllReelsQueryModel query);

        ICollection<string> GetReelBrands();

        Brand GetReelBrandByName(AddReelFormModel reel);

        void CreateReel(AddReelFormModel reel, Brand brand);

        Reel GetReelById(int id);

        AddReelFormModel GetReelEditModel(int id);

        void EditReel(int id, AddReelFormModel item);

        void DeleteReel(Reel reel);

        void DecrementReelQuantity(Reel currReel);
    }
}
