using NZWorks.API.Data;
using NZWorks.API.Models.Domain;

namespace NZWorks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NZWalksDBContext _dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalksDBContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;
        }
    }
}
