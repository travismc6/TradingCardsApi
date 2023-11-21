using CloudinaryDotNet.Actions;
using OfficeOpenXml;
using System.IO;
using TradingCards.Models.Domain;
using TradingCards.Models.Dtos;

namespace TradingCards.Services
{
    public class ImportExportService : IImportExportService
    {
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

        public IEnumerable<ChecklistCardDto> ImportCollection(IFormFile file)
        {
            var cards = new List<Card>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        //cards.Add(new Card
                        //{
                        //    Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                        //    Age = int.Parse(worksheet.Cells[row, 2].Value.ToString().Trim()),
                        //    Email = worksheet.Cells[row, 3].Value.ToString().Trim()
                        //});
                    }
                }
            }




            throw new NotImplementedException();
        }
    }
}
