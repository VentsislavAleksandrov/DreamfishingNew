namespace DreamFishingNew.Data.Models
{
    public class Reel
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        public string Image { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int Weight { get; set; }

        public int? ProductCartId { get; set; }

        public ProductCart ProductCart { get; set; }

        public int BalbearingsCount { get; set; }

        public string GearRatio { get; set; }

        public string LineCapacity { get; set; }
    }
}
