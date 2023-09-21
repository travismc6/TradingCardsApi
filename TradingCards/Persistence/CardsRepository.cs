using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradingCards.Models.Domain;
using TradingCards.Models.Dtos;

namespace TradingCards.Persistence
{
    public class CardsRepository : ICardsRepository
    {
        private readonly ApplicationDbContext _context;
        public CardsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<CardDto>> GetCards([FromQuery] CardParams userParams)
        {
            return await (from card in _context.Cards
                          where (!userParams.Year.HasValue || card.CardSet.Year == userParams.Year.Value)
                          &&  (userParams.Brands == null || !userParams.Brands!.Any() || userParams.Brands!.Contains(card.CardSet.BrandId) )
                          && (string.IsNullOrEmpty(userParams.Name) || card.Name.ToLower().Contains(userParams.Name.ToLower()))
                          join collectionCard in _context.CollectionCards
                         on card.Id equals collectionCard.CardId into collectionGroup
                         from collectionCard in collectionGroup.DefaultIfEmpty()
                         select new CardDto
                         {
                             Id = card.Id,
                             Name = card.Name,
                             Notes = card.Notes,
                             Number = card.Number,
                             SetName = card.CardSet.Name,
                             BrandId = card.CardSet.BrandId,
                             Year = card.CardSet.Year,
                             InCollection = collectionCard != null
                         }).ToListAsync();
        }

        public async Task<bool> SaveCollection(CollectionChangesDto collectionChanges)
        {
            // TODO get actual collection from Auth
            var collection = _context.Collections.First();

            foreach (var cardId in collectionChanges.Added)
            {
                var existing = _context.CollectionCards.Where(c => c.CardId == cardId && c.CollectionId == collection.Id);
                if (!existing.Any())
                {
                    var collectionCard = new CollectionCard
                    {
                        CardId = cardId,
                        CollectionId = collection.Id,
                    };
                    _context.CollectionCards.Add(collectionCard);
                }
                
            }

            var removedCards = _context.CollectionCards.Where(r => collectionChanges.Removed.Contains(r.Id));
            _context.CollectionCards.RemoveRange(removedCards);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
