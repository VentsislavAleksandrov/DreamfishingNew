using DreamFishingNew.Data.Models;
using DreamFishingNew.Models.Lines;
using System.Collections.Generic;

namespace DreamFishingNew.Services.Lines
{
    public interface ILineService
    {
        ICollection<Line> GetAllLines();

        ICollection<Line> GetLinesByBrand( ICollection<Line> linesQuery, AllLinesQueryModel query);

        ICollection<Line> GetLinesBySearchTerm( ICollection<Line> linesQuery, AllLinesQueryModel query);

        ICollection<Line> GetLinesBySortTerm( ICollection<Line> linesQuery, AllLinesQueryModel query);

        ICollection<LineListingViewModel> GetLinesByPage( ICollection<Line> linesQuery, AllLinesQueryModel query);

        ICollection<string> GetLineBrands();

        Brand GetLineBrandByName(AddLineFormModel line);

        void CreateLine(AddLineFormModel line, Brand brand);

        Line GetLineById(int id);

        AddLineFormModel GetLineEditModel(int id);

        void EditLine(int id, AddLineFormModel item);

        void DeleteLine(Line line);

        void DecrementLineQuantity(Line currLine);
    }
}
