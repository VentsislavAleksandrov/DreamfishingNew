using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Reels
{
    public class AddReelFormModel
    {
        [Required]
        public string Model { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Image { get; set; }

        [Range(1,10000)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(1,10000)]
        public int Quantity { get; set; }

        [Range(1,2000)]
        public int Weight { get; set; }

        [Range(1,16)]
        public int BalbearingsCount { get; set; }

        [Required]
        public string GearRatio { get; set; }

        [Required]
        public string LineCapacity { get; set; }
    }
}
