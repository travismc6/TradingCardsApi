using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TradingCards.Helpers;
using TradingCards.Models.Domain;
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
        private readonly IImportExportService _exportService;


        public CardCollectionController(ICardsRepository repo, IImageService imageService, IImportExportService exportService)
        {
            _repo = repo;
            _imageService = imageService;
            _exportService = exportService;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollectionCardByCardId(int id)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst("id")?.Value;

            var card = await _repo.GetCollectionCard(id, userId);

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

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteCollectionCard(int id)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst("id")?.Value;

            var result = await _repo.DeleteCard(id, userId);

            return Ok();
        }

        [HttpPost("duplicate/{id}")]
        public async Task<IActionResult> DuplicateCollectionCard(int id)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst("id")?.Value;

            var result = await _repo.DuplicateCard(id, userId);

            return Ok(result);
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


        [HttpGet("details")]
        public async Task<IActionResult> Details(int id)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst("id")?.Value;

            var result = await _repo.GetCollectionSetDetails(id, userId);

            return Ok(result);
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

        [HttpPost("import")]
        public async Task<IActionResult> ImportCards(IFormFile file)
        {
            //// TODO: Must have admin role

            //if (file == null || file.Length == 0)
            //{
            //    return BadRequest("No file uploaded.");
            //}

            //CardSet set;
            //using (var reader = new StreamReader(file.OpenReadStream()))
            //{
            //    var fileContent = await reader.ReadToEndAsync();
            //    set = JsonConvert.DeserializeObject<CardSet>(fileContent);
            //    set.BrandId = (await _db.Brands.FirstOrDefaultAsync(r => set.Name.StartsWith(r.Name))).Id;
            //}

            //if (_db.CardSets.Any(r => r.BrandId == set.BrandId && r.Year == set.Year && r.Name == set.Name))
            //{
            //    return BadRequest("Set already exists");
            //}
            //await _db.CardSets.AddAsync(set);
            //await _db.SaveChangesAsync();

            //return Ok(set);
            return Ok();
        }
    }
}
