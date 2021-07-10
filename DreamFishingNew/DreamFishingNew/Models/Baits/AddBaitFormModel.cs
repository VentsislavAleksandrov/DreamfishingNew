using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Baits
{
    public class AddBaitFormModel
    {
        [Required]
        public string Model { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Image { get; set; }

        [Range(1,10000)]
        public int Quantity { get; set; }

        [Range(1,1000)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(1,50)]
        public int Weight { get; set; }

        [Range(1,40)]
        public int Length { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
