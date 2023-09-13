using Microsoft.AspNetCore.Mvc;
using TradingCards.Models.Dtos;
using TradingCards.Persistence;

namespace TradingCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardsRepository _repo;

        public CardsController(ICardsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCards([FromQuery] CardParams cardParams)
        {
            var cards = await _repo.GetCards(cardParams);

            return Ok(cards);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCollectionCards(CollectionChangesDto collectionChanges)
        {
            await _repo.SaveCollection(collectionChanges);

            return Ok();
        }
    }
}
