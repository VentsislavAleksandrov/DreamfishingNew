using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Bags
{
    public class AllbagsQueryModel
    {
        public const int BagsPerPage = 6;

        public string Brand { get; set; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }

        public BagSorting Sorting { get; set; }

        public int currentPage { get; set; } = 1;

        public IEnumerable<BagListingViewModel> Bags { get; set; }
    }
}
