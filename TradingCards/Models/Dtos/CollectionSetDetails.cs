namespace TradingCards.Models.Dtos
{
    public class CollectionSetDetails
    {
        public int SetId { get; set; }
        public string SetName { get; set; }
        public int SetYear { get; set; }
        public int SetCount { get; set; }
        public int CollectionCount { get; set; }
        public int UniqueCollectionCount { get; set; }
        public int BrandId { get; set; }
    }
}
