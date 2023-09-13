using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TradingCards.Models.Domain;
using TradingCards.Persistence;

namespace TradingCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> UploadCardSet(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            CardSet set;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = await reader.ReadToEndAsync();
                set = JsonConvert.DeserializeObject<CardSet>(fileContent);
                set.BrandId = (await _db.Brands.FirstOrDefaultAsync(r => set.Name.StartsWith(r.Name))).Id;
            }

            if(_db.CardSets.Any(r => r.BrandId == set.BrandId && r.Year == set.Year && r.Name == set.Name))
            {
                return BadRequest("Set already exists");
            }
            await _db.CardSets.AddAsync(set);
            await _db.SaveChangesAsync();

            return Ok(set);
        }
    }
}
