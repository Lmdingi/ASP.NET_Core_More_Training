using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWorks.API.Models.Domain;
using NZWorks.API.Models.DTO;
using NZWorks.API.Repositories;

namespace NZWorks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        #region upload new image
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDto request)
        {
            ValidateFileUpload(request);

            if(ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };

                await _imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);
        }
        #endregion

        #region validation
        private void ValidateFileUpload(ImageUploadDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png"};

            if(!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsurpported file");
            }

            if(request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10mb, please upload a smaller size file.");
            }
        }
        #endregion
    }
}
