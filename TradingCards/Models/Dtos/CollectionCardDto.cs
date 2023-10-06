namespace TradingCards.Models.Dtos
{
    public class CollectionCardDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string? Notes { get; set; }
        public int Year { get; set; }
        public string SetName { get; set; }
        public string BrandName { get; set; }
        public double? Grade { get; set; }
        public string? FrontImageUrl { get; set; }
        public string? BackImageUrl { get; set; }

        public string? DefaultFrontImageUrl { get; set; }
        public string? DefaultBackImageUrl { get; set; }

        public string? FrontImagePublicId { get; set; }
        public string? BackImagePublicId { get; set; }
    }
}
