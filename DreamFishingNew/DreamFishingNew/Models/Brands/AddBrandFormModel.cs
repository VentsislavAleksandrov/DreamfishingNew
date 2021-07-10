using System.ComponentModel.DataAnnotations;

namespace DreamFishingNew.Models.Brands
{
    public class AddBrandFormModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
    }
}
