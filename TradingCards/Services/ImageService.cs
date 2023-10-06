using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace TradingCards.Services
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;

        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;

            var acc = new Account(
                _configuration["Cloudinay:CloudName"],
                _configuration["Cloudinay:ApiKey"],
                _configuration["Cloudinay:ApiSecret"]
           );

            _cloudinary = new Cloudinary( acc );
        }

        public async Task<ImageUploadResult> AddImage(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }
    }
}
