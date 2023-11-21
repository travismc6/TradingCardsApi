using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace TradingCards.Models.Domain
{
    public class Card
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string? Notes { get; set; }
        public int CardSetId { get; set; }

        public string? FrontImageUrl { get; set; }
        public string? BackImageUrl { get; set; }


        [JsonIgnore]
        public CardSet CardSet { get; set; }

        //TODO
        //public Photo Photo { get; set; }
        // PlayerId
        // TeamId
        // public string Team { get; set; }
    }
}
