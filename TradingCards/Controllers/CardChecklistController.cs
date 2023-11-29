using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingCards.Models.Dtos;
using TradingCards.Persistence;

namespace TradingCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardChecklistController : ControllerBase
    {
        private readonly ICardsRepository _repo;

        public CardChecklistController(ICardsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCardChecklist([FromQuery] CardParams cardParams)
        {
            var user = HttpContext.User;
            cardParams.UserId = user.FindFirst("id")?.Value; 

            var cards = await _repo.GetChecklistCards(cardParams);

            return Ok(cards);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveChecklistCards(CollectionChangesDto collectionChanges)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst("id")?.Value; 

            if(userId == null)
            {
                return BadRequest("User not found");
            }

            collectionChanges.UserId = userId;

            await _repo.SaveCollection(collectionChanges);

            return Ok();
        }

    }
}
