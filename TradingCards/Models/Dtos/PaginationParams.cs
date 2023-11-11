namespace TradingCards.Models.Dtos
{
    public class PaginationParams
    {
        public int MaxPageSize { get; set; }
        public int PageNumber { get; set; } = 1;
    }
}
