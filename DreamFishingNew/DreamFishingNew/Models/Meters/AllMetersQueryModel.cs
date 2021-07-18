using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Meters
{
    public class AllMetersQueryModel
    {
        public const int MetersPerPage = 6;

        public string Brand { get; set; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }

        public MeterSorting Sorting { get; set; }

        public int currentPage { get; set; } = 1;

        public IEnumerable<MeterListingViewModel> Meters { get; set; }
    }
}
