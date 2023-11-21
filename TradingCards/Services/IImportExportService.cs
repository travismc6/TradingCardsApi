using CloudinaryDotNet.Actions;
using TradingCards.Models.Dtos;

namespace TradingCards.Services
{
    public interface IImportExportService
    {
        public MemoryStream ExportCollection(List<ChecklistCardDto> cards);
        public IEnumerable<ChecklistCardDto> ImportCollection(IFormFile file);
    }
}
