using CloudinaryDotNet.Actions;
using OfficeOpenXml;
using System.IO;
using TradingCards.Models.Domain;
using TradingCards.Models.Dtos;
using TradingCards.Persistence;

namespace TradingCards.Services
{
    public class ImportExportService : IImportExportService
    {
        private ICardsRepository _cardsRepository { get; set; }

        public ImportExportService(ICardsRepository cardsRepository)
        {
            _cardsRepository = cardsRepository;
        }

        public MemoryStream ExportCollection(List<ChecklistCardDto> cards)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Cards");

                worksheet.Column(4).Width = 25;
                worksheet.Column(5).Width = 30;
                worksheet.Column(7).Width = 25;
                worksheet.Column(8).Width = 25;

                // Headers
                worksheet.Cells[1, 1].Value = "#";
                worksheet.Cells[1, 2].Value = "Year";
                worksheet.Cells[1, 3].Value = "Brand";
                worksheet.Cells[1, 4].Value = "Set";
                worksheet.Cells[1, 5].Value = "Player Name";
                worksheet.Cells[1, 6].Value = "Notes";
                worksheet.Cells[1, 7].Value = "Grade";
                worksheet.Cells[1, 8].Value = "FrontImageUrl";
                worksheet.Cells[1, 9].Value = "BackImageUrl";

                // Insert data
                for (int i = 0; i < cards.Count; i++)
                {
                    var currentRow = i + 2; // +2 because headers are on the first row

                    worksheet.Cells[currentRow, 1].Value = cards[i].Number;
                    worksheet.Cells[currentRow, 2].Value = cards[i].Year;
                    worksheet.Cells[currentRow, 3].Value = cards[i].BrandName;
                    worksheet.Cells[currentRow, 4].Value = cards[i].SetName;
                    worksheet.Cells[currentRow, 5].Value = cards[i].Name;
                    worksheet.Cells[currentRow, 6].Value = cards[i].CollectionCardNotes;
                    worksheet.Cells[currentRow, 7].Value = cards[i].Grade;
                    worksheet.Cells[currentRow, 8].Value = cards[i].FrontImageUrl;
                    worksheet.Cells[currentRow, 9].Value = cards[i].BackImageUrl;
                }
                package.Save();
            }

            stream.Position = 0;
            return stream;
        }

        public async Task<ImportDetailsDto> ImportCollection(IFormFile file, string userId)
        {
            var importDetails = new ImportDetailsDto();

            var collection = await _cardsRepository.GetCollection(userId);

            var cards = new List<CollectionCard>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var number = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                        var year = worksheet.Cells[row, 2].Value;
                        var brand = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                        var set = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                        var name = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
                        var notes = worksheet.Cells[row, 6].Value?.ToString()?.Trim();
                        var grade = worksheet.Cells[row, 7].Value?.ToString()?.Trim();
                        var frontImageUrl = worksheet.Cells[row, 8].Value?.ToString()?.Trim();
                        var backImageUrl = worksheet.Cells[row, 9].Value?.ToString()?.Trim();

                        var card = await _cardsRepository.FindCard(number, Convert.ToInt32(year), brand, set);

                        if (card != null)
                        {
                            var collectionCard = new CollectionCard()
                            {
                                CardId = card.Id,
                                Grade = grade != null ? Convert.ToDouble(grade) : null,
                                CollectionId = collection.Id,
                                BackImageUrl = backImageUrl,
                                FrontImageUrl = frontImageUrl,
                                Notes = notes
                            };

                            cards.Add(collectionCard);
                            importDetails.Imported++;
                        } 
                        else
                        {
                            importDetails.Failed++;
                        }
                    }

                    // save
                    var result = await _cardsRepository.SaveCollection(cards, userId, true);
                }
            }

            return importDetails;
        }
    }
}
