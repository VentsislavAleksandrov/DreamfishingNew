using System.Collections.Generic;

namespace DreamFishingNew.Data.Models
{
    public class ProductCart
    {
        public int Id { get; set; }


        public ICollection<Bag> Bags { get; set; } = new List<Bag>();

        public ICollection<Bait> Baits { get; set; } = new List<Bait>();

        public ICollection<Clothes> Clothes { get; set; } = new List<Clothes>();

        public ICollection<Glasses> Glasses { get; set; } = new List<Glasses>();

        public ICollection<Line> Lines { get; set; } = new List<Line>();

        public ICollection<Meter> Meters { get; set; } = new List<Meter>();

        public ICollection<Reel> Reels { get; set; } = new List<Reel>();

        public ICollection<Rod> Rods { get; set; } = new List<Rod>();
    }
}
