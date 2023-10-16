using TradingCards.Models.Dtos;

namespace TradingCards.Persistence
{
    public interface ICardsRepository
    {
        Task<ICollection<ChecklistCardDto>> GetChecklistCards(CardParams userParams);
        //Task<CollectionCardDto> GetCollectionCardByCardId(int id, string userId);
        Task<CollectionCardDto> GetCollectionCard(int id, string userId);
        Task<CollectionCardDto> SaveCard(CollectionCardDto card, string userId);
        Task<bool> DeleteCard(int id, string userId);
        Task<int> DuplicateCard(int id, string userId);
        Task<bool> SaveCollection(CollectionChangesDto collectionChanges);
        Task<CollectionDetailsDto> GetCollectionSetDetails(int id, string userId);
    }
}
