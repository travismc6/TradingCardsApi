namespace TradingCards.Models.Dtos
{
    public class CardParams
    {
        public int? Year { get; set; }
        public List<int>? Brands { get; set; }
        public string? Name { get; set; }
        public string? UserId { get; set; }
        public bool InCollection { get; set; }
    }
}
