using DreamFishingNew.Models.Rods;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Reels
{
    public class AllReelsQueryModel
    {
         public const int ReelsPerPage = 6;

        public string Brand { get; set; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }

        public ReelSorting Sorting { get; set; }

        public int currentPage { get; set; } = 1;

        public IEnumerable<ReelListingViewModel> Reels { get; set; }
    }
}
