using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Glasses
{
    public class AllGlassesQueryModel
    {
        public const int GlassesPerPage = 6;

        public string Brand { get; set; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }

        public GlassesSorting Sorting { get; set; }

        public int currentPage { get; set; } = 1;

        public IEnumerable<GlassesListingViewModel> Glasses { get; set; }
    }
}
