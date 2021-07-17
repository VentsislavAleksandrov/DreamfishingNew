using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Rods
{
    public class AllRodsQueryModel
    {
        public const int RodsPerPage = 6;

        public string Brand { get; set; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }

        public RodSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public IEnumerable<RodListingViewModel> Rods { get; set; }
    }
}
