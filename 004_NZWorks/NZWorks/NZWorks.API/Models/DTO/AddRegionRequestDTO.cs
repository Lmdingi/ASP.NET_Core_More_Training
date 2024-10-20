using System.ComponentModel.DataAnnotations;

namespace NZWorks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code minimum must be 3 letters")]
        [MaxLength(3, ErrorMessage = "Code maximum must be 3 letters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name must not be more than 100 letters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
