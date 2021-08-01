using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Meters;
using System.Collections.Generic;

namespace DreamFishingNew.Services.Meters
{
    public interface IMeterService
    {
        ICollection<Meter> GetAllMeters();

        ICollection<Meter> GetMetersByBrand( ICollection<Meter> metersQuery, AllMetersQueryModel query);

        ICollection<Meter> GetMetersBySearchTerm( ICollection<Meter> metersQuery, AllMetersQueryModel query);

        ICollection<Meter> GetMetersBySortTerm( ICollection<Meter> metersQuery, AllMetersQueryModel query);

        ICollection<MeterListingViewModel> GetMetersByPage( ICollection<Meter> metersQuery, AllMetersQueryModel query);

        ICollection<string> GetMeterBrands();

        Brand GetMeterBrandByName(AddMeterFormModel meter);

        void CreateMeter(AddMeterFormModel meter, Brand brand);

        Meter GetMeterById(int id);

        AddMeterFormModel GetMeterEditModel(int id);

        void EditMeter(int id, AddMeterFormModel item);

        void DeleteMeter(Meter meter);
    }
}
