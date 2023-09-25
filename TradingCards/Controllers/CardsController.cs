using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            var user = HttpContext.User;
            cardParams.UserId = user.FindFirst("id")?.Value; 

            var cards = await _repo.GetCards(cardParams);

            return Ok(cards);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCollectionCards(CollectionChangesDto collectionChanges)
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
