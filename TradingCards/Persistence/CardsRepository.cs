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

        public async Task<CollectionCardDto> GetCollectionCardByCardId(int id, string userId)
        {
            // if card is not in collection (or another collection), return error
            var collection = _context.Collections.FirstOrDefault(u => u.UserId == userId);

            var collectionCard = await _context.CollectionCards.Include(r => r.Card).Include(r => r.Card.CardSet).FirstOrDefaultAsync(r => r.CollectionId == collection.Id && r.CardId == id);

            if (collectionCard == null)
            {
                throw new InvalidOperationException("Card not found.");
            }

            return await GetCollectionCard(collectionCard.Id, userId);
        }

        public async Task<CollectionCardDto> GetCollectionCard(int id, string userId)
        {
            // if card is not in collection (or another collection), return error
            var collection = _context.Collections.FirstOrDefault(u => u.UserId == userId);

            var collectionCard = await _context.CollectionCards.Include(r => r.Card).Include(r => r.Card.CardSet).FirstOrDefaultAsync(r => r.CollectionId == collection.Id && r.Id == id);

            if (collectionCard == null)
            {
                throw new InvalidOperationException("Card is not in your collection.");
            }

            return new CollectionCardDto
            {
                Id = collectionCard.Id,
                Name = collectionCard.Card.Name,
                Notes = collectionCard.Notes,
                Number = collectionCard.Card.Number,
                SetName = collectionCard.Card.CardSet.Name,
                BrandName = collectionCard.Card.CardSet.Name,
                Grade = collectionCard.Grade,
                Year = collectionCard.Card.CardSet.Year,
                FrontImageUrl = collectionCard.FrontImageUrl,
                BackImageUrl = collectionCard.BackImageUrl,
                DefaultFrontImageUrl = collectionCard.Card.CardSet.ExternalImageFrontBaseUrl,
                DefaultBackImageUrl = collectionCard.Card.CardSet.ExternalImageBackBaseUrl,
            };
        }

        public async Task<CollectionCardDto> SaveCard(CollectionCardDto card, string userId)
        {
            var collection = _context.Collections.FirstOrDefault(u => u.UserId == userId);

            var collectionCard = await _context.CollectionCards.FirstOrDefaultAsync(r => r.Id == card.Id);

            if (collectionCard == null)
            {
                throw new InvalidOperationException("Card is not in your collection.");
            }

            collectionCard.FrontImageUrl = card.FrontImageUrl;
            collectionCard.BackImageUrl = card.BackImageUrl;
            collectionCard.FrontImagePublicId = card.FrontImagePublicId;
            collectionCard.BackImagePublicId = card.BackImagePublicId;
            collectionCard.Notes = card.Notes;
            collectionCard.Grade = card.Grade;

            await _context.SaveChangesAsync();

            return card;
        }

        public async Task<ICollection<ChecklistCardDto>> GetCards([FromQuery] CardParams userParams)
        {
            int? collectionId = null;

            if(userParams.UserId != null)
            {
                var collection = _context.Collections.FirstOrDefault(u => u.UserId == userParams.UserId);

                if (collection == null)
                {
                    collection = new Collection()
                    {
                        Name = "My Collection",
                        UserId = userParams.UserId
                    };

                    _context.Collections.Add(collection);
                    await _context.SaveChangesAsync();
                }

                collectionId = collection!.Id;
            }

            if (collectionId == null)
            {
                return await (from card in _context.Cards
                              where (!userParams.Year.HasValue || card.CardSet.Year == userParams.Year.Value)
                              && (userParams.Brands == null || !userParams.Brands!.Any() || userParams.Brands!.Contains(card.CardSet.BrandId))
                              && (string.IsNullOrEmpty(userParams.Name) || card.Name.ToLower().Contains(userParams.Name.ToLower()))
                              select new ChecklistCardDto
                              {
                                  Id = card.Id,
                                  Name = card.Name,
                                  Notes = card.Notes,
                                  Number = card.Number,
                                  SetName = card.CardSet.Name,
                                  BrandId = card.CardSet.BrandId,
                                  Year = card.CardSet.Year,
                                  InCollection = false
                              }).ToListAsync();
            }
            else
            {
                return await (from card in _context.Cards
                              where (!userParams.Year.HasValue || card.CardSet.Year == userParams.Year.Value)
                              && (userParams.Brands == null || !userParams.Brands!.Any() || userParams.Brands!.Contains(card.CardSet.BrandId))
                              && (string.IsNullOrEmpty(userParams.Name) || card.Name.ToLower().Contains(userParams.Name.ToLower()))
                              join collectionCard in _context.CollectionCards.Where(r => r.CollectionId == collectionId)
                             on card.Id equals collectionCard.CardId into collectionGroup
                              from collectionCard in collectionGroup.DefaultIfEmpty()
                              where (!userParams.InCollection || collectionCard != null)
                              select new ChecklistCardDto
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
        }

        public async Task<bool> SaveCollection(CollectionChangesDto collectionChanges)
        {
            var collection = _context.Collections.FirstOrDefault(u => u.UserId == collectionChanges.UserId);

            if (collectionChanges.Added != null)
            {
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
            }

            if (collectionChanges.Removed != null) { 

                var removedCards = _context.CollectionCards.Where(r => collectionChanges.Removed.Contains(r.Id));
                _context.CollectionCards.RemoveRange(removedCards);
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
