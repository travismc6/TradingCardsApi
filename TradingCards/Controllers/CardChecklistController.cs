using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TradingCards.Helpers;
using TradingCards.Models.Dtos;
using TradingCards.Persistence;
using TradingCards.Services;
using static System.Net.Mime.MediaTypeNames;

namespace TradingCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardChecklistController : ControllerBase
    {
        private readonly ICardsRepository _repo;
        private readonly IExportService _exportService;

        public CardChecklistController(ICardsRepository repo, IExportService exportService)
        {
            _repo = repo;
            _exportService = exportService;
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

        [HttpGet("export")]
        public async Task<IActionResult> ExportChecklist([FromQuery] CardParams cardParams)
        {
            var user = HttpContext.User;
            cardParams.UserId = user.FindFirst("id")?.Value;
            cardParams.InCollection = true;

            var cards = await _repo.GetChecklistCards(cardParams);

            var result = _exportService.ExportCollection(cards.ToList());

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "cards.xlsx");
        }

    }
}
