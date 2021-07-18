using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Baits
{
    public class AllBaitsQueryModel
    {
        public const int BaitsPerPage = 6;

        public string Brand { get; set; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }

        public BaitSorting Sorting { get; set; }

        public int currentPage { get; set; } = 1;

        public IEnumerable<BaitListingViewModel> Baits { get; set; }
    }
}
