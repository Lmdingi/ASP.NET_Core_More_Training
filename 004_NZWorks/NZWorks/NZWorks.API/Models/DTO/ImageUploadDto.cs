using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWorks.API.Models.DTO
{
    public class ImageUploadDto
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
    }
}
