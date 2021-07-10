using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DreamFishingNew.Models.Glasses
{
    public class AddGlassesFormModel
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

        [Range(1,100)]
        public int Weight { get; set; }
    }
}
