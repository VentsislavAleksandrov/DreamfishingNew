using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Rods
{
    public class AddRodFormModel
    {
        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        [Required]
        [MaxLength(50)]
        public string Brand { get; set; }

        [Range(1,10000)]
        public int Quantity { get; set; }

        [Required]
        public string Image { get; set; }

        [Range(1,10000)]
        public decimal Price { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Range(1,1000)]
        public int Weight { get; set; }

        [Range(1,2000)]
        public int Length { get; set; }

        [Range(1,5)]
        public int PartsCount { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string CastingWeight { get; set; }
    }
}
