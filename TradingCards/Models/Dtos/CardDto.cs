namespace TradingCards.Models.Dtos
{
    public class CardDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int Year { get; set; }
        public string SetName { get; set; }
        public int BrandId { get; set; }
        public bool InCollection { get; set; }
    }
}
