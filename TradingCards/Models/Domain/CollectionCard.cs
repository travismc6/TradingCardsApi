namespace TradingCards.Models.Domain
{
    public class CollectionCard
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public double? Grade { get; set; }
        public string? Notes { get; set; }
        public int CollectionId { get; set; }
        public string? FrontImageUrl { get; set; }
        public string? BackImageUrl { get; set; }

        public string? FrontImagePublicId { get; set; }
        public string? BackImagePublicId { get; set; }
        public Collection Collection { get; set; }

        public Card Card { get; set; }
    }
}
