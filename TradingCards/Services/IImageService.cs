using CloudinaryDotNet.Actions;

namespace TradingCards.Services
{
    public interface IImageService
    {
        public Task<ImageUploadResult> AddImage(IFormFile file);
    }
}
