namespace TradingCards.Models.Domain
{
    public class CardSet
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ExternalLinkUrl { get; set; }
        public string? ExternalImageFrontBaseUrl { get; set; }
        public string? ExternalImageBackBaseUrl { get; set; }


        public ICollection<Card> Cards { get; set; }
    }
}
