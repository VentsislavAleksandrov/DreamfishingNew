using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Clothes
{
    public class AllClothesQueryModel
    {
        public const int ClothesPerPage = 6;

        public string Brand { get; set; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }

        public ClothesSorting Sorting { get; set; }

        public int currentPage { get; set; } = 1;

        public IEnumerable<ClothesListingViewModel> Clothes { get; set; }
    }
}
