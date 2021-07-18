using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamFishingNew.Models.Lines
{
    public class AllLinesQueryModel
    {
        public const int LinesPerPage = 6;

        public string Brand { get; set; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name ="Search")]
        public string SearchTerm { get; set; }

        public LineSorting Sorting { get; set; }

        public int currentPage { get; set; } = 1;

        public IEnumerable<LineListingViewModel> Lines { get; set; }
    }
}
