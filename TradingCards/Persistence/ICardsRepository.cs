using TradingCards.Models.Dtos;

namespace TradingCards.Persistence
{
    public interface ICardsRepository
    {
        Task<ICollection<CardDto>> GetCards(CardParams userParams);
        Task<bool> SaveCollection(CollectionChangesDto collectionChanges);
    }
}
