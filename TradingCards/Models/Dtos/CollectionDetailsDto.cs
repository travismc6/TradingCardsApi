namespace TradingCards.Models.Dtos
{
    public class CollectionDetailsDto
    {
        public int Id { get; set; }
        public int TotalCards { get; set; }
        public List<CollectionSetDetails> CollectionSets { get; set; }
    }

}
