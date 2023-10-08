using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradingCards.Helpers;
using TradingCards.Models.Dtos;
using TradingCards.Persistence;
using TradingCards.Services;

namespace TradingCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardCollectionController : ControllerBase
    {
        private readonly ICardsRepository _repo;
        private readonly IImageService _imageService;

        public CardCollectionController(ICardsRepository repo, IImageService imageService)
        {
            _repo = repo;
            _imageService = imageService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollectionCardByCardId(int id)
        {
            var userName = User.Identity?.Name;

            var user = HttpContext.User;
            var userId = user.FindFirst("id")?.Value;

            var card = await _repo.GetCollectionCardByCardId(id, userId);

            return Ok(card);
        }

        [HttpPut]
        public async Task<IActionResult> SaveCollectionCard(CollectionCardDto card)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst("id")?.Value;

            var cards = await _repo.SaveCard(card, userId);

            return Ok(cards);
        }

        [HttpPost("image/{id}")]
        public async Task<IActionResult> UploadImageAsync(int id, IFormFile file, [FromQuery] ImageTypeEnum imageType)
        {
            try
            {
                var userName = User.Identity?.Name;

                var user = HttpContext.User;
                var userId = user.FindFirst("id")?.Value;

                var card = await _repo.GetCollectionCard(id, userId);


                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid file");
                }

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Add more if needed
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid file type");
                }

                var uploadResult = await _imageService.AddImage(file);

                if(uploadResult.Error != null)
                {
                    return BadRequest(uploadResult.Error.Message);
                }

                if(imageType == ImageTypeEnum.Front)
                {
                    card.FrontImageUrl = uploadResult.SecureUrl.ToString();
                    card.FrontImagePublicId = uploadResult.PublicId;
                }
                else
                {
                    card.BackImageUrl = uploadResult.SecureUrl.ToString();
                    card.BackImagePublicId = uploadResult.PublicId;
                }

                await _repo.SaveCard(card, userId);

                return Ok(uploadResult.SecureUrl.ToString());
            }
            catch (Exception ex)
            {
                // Log the exception for debugging and monitoring
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
