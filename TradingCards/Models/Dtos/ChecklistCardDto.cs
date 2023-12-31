﻿namespace TradingCards.Models.Dtos
{
    public class ChecklistCardDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string? CollectionCardNotes { get; set; }
        public int? CollectionCardId { get; set; }
        public int Year { get; set; }
        public string SetName { get; set; }
        public double? Grade { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public bool InCollection { get; set; }
        public string? FrontImageUrl { get; set; }
        public string? BackImageUrl { get; set; }
    }
}
