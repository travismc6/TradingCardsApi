namespace TradingCards.Models.Dtos
{
    public class CollectionChangesDto
    {
        public List<int>? Added { get; set; }
        public List<int>? Removed { get; set; }

        public string? UserId { get; set; }
    }
}
