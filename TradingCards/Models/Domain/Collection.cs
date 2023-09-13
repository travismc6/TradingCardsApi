namespace TradingCards.Models.Domain
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public ICollection<CollectionCard> CollectionCards { get; set; }
    }
}
