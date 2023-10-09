using CloudinaryDotNet.Actions;
using TradingCards.Models.Dtos;

namespace TradingCards.Services
{
    public interface IExportService
    {
        public MemoryStream ExportCollection(List<ChecklistCardDto> cards);
    }
}
