using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Lines
{
    public class AddLineFormModel
    {
        [Required]
        public string Model { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Image { get; set; }

        [Range(1,10000)]
        public int Quantity { get; set; }

        [Range(1,10000)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(1,1000)]
        public int Weight { get; set; }

        [Required]
        public string Size { get; set; }

        [Range(1,1000)]
        public int Length { get; set; }
    }
}
